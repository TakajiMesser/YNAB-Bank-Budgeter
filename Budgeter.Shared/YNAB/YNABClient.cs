using Budgeter.Shared.YNAB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Budgeter.Shared.YNAB
{
    public class YNABClient : TransactionSet<YNABTransaction>
    {
        public const string BASE_URL = "https://api.youneedabudget.com/v1";

        private HttpClient _client = new HttpClient();

        public YNABClient(string personalAccessToken)
        {
            PersonalAccessToken = personalAccessToken;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", PersonalAccessToken);
        }

        public string PersonalAccessToken { get; }

        public void FetchTransactions()
        {
            var budgetsTask = GetBudgetsAsync();
            budgetsTask.Wait();
            var budget = budgetsTask.Result.First();

            var transactionsTask = GetTransactionsAsync(budget.ID);
            transactionsTask.Wait();
            var transactions = transactionsTask.Result;

            foreach (var transaction in transactions)
            {
                _transactions.Add(new YNABTransaction()
                {
                    Amount = transaction.Amount / 1000.0f,
                    Approved = transaction.Approved,
                    CategoryName = transaction.CategoryName,
                    Cleared = transaction.Cleared,
                    Date = DateTime.Parse(transaction.Date),
                    FlagColor = transaction.FlagColor,
                    Memo = transaction.Memo,
                    PayeeName = transaction.PayeeName
                });
            }
        }

        public async Task<IEnumerable<Budget>> GetBudgetsAsync()
        {
            var response = await _client.GetAsync(BASE_URL + "/budgets");
            return await ParseResponseForModels<Budget>(response);
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync(string budgetID)
        {
            var response = await _client.GetAsync(BASE_URL + "/budgets/" + budgetID + "/accounts");
            return await ParseResponseForModels<Account>(response);
        }

        public async Task<IEnumerable<Budgeter.Shared.YNAB.Models.Transaction>> GetTransactionsAsync(string budgetID)
        {
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, 1);

            var response = await _client.GetAsync(BASE_URL + "/budgets/" + budgetID + "/transactions?since_date=" + date.ToString("yyyy-MM-dd"));
            return await ParseResponseForModels<Budgeter.Shared.YNAB.Models.Transaction>(response);
        }

        private async Task<T> ParseResponseForModel<T>(HttpResponseMessage response) where T : Model
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);

            if (!response.IsSuccessStatusCode) throw new YNABException(json["error"].Value<Error>());

            var data = json["data"];
            var resourceName = GetResourceName<T>(false);

            return data[resourceName].Value<T>();
        }

        private async Task<List<T>> ParseResponseForModels<T>(HttpResponseMessage response) where T : Model
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy { ProcessDictionaryKeys = true }
                }
            };

            var json = JsonConvert.DeserializeObject<JObject>(responseString, settings);
            //var json = JObject.Parse(responseString);

            if (!response.IsSuccessStatusCode) throw new YNABException(json["error"].Value<Error>());

            var data = json["data"];
            var resourceName = GetResourceName<T>(true);

            var resource = data[resourceName];

            var resourceValue = JsonConvert.DeserializeObject<T[]>(resource.ToString());
            return resourceValue.ToList();

            //return data[resourceName].Value<T[]>().ToList();
        }

        private string GetResourceName<T>(bool isPlural = false) where T : Model
        {
            var builder = new StringBuilder();
            var typeName = typeof(T).Name;

            for (var i = 0; i < typeName.Length; i++)
            {
                if (i == 0)
                {
                    builder.Append(char.ToLower(typeName[i]));
                }
                else if (isPlural && i == typeName.Length - 1)
                {
                    if (typeName[i] == 'y')
                    {
                        builder.Append("ies");
                    }
                    else
                    {
                        builder.Append(typeName[i]);
                        builder.Append("s");
                    }
                }
                else
                {
                    builder.Append(typeName[i]);
                }
            }

            return builder.ToString();
        }
    }
}
