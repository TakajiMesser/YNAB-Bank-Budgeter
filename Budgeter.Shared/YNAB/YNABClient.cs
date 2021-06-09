using Budgeter.Shared.YNAB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Budgeter.Shared.YNAB
{
    public class YNABClient : Client<YNABTransaction>
    {
        public const string BASE_URL = "https://api.youneedabudget.com/v1";

        private YNABConfiguration _configuration;
        private HttpClient _client = new();

        public YNABClient(YNABConfiguration configuration)
        {
            _configuration = configuration;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.PersonalAccessToken);
        }

        public override async Task FetchTransactions()
        {
            var budget = await GetBudgetAsync();
            var accounts = await GetAccountAsync(budget);

            foreach (var account in accounts)
            {
                var transactions = await GetTransactionsAsync(budget, account);

                foreach (var transaction in transactions)
                {
                    _transactions.Add(new YNABTransaction()
                    {
                        AccountName = account.Name,
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
        }

        public async Task<Budget> GetBudgetAsync()
        {
            var response = await _client.GetAsync(BASE_URL + "/budgets");
            var budgets = await ParseResponseForModels<Budget>(response);

            return budgets.FirstOrDefault(b => b.Name == _configuration.BudgetName);
        }

        public async Task<IEnumerable<Account>> GetAccountAsync(Budget budget)
        {
            var response = await _client.GetAsync(BASE_URL + "/budgets/" + budget.ID + "/accounts");
            var accounts = await ParseResponseForModels<Account>(response);

            return accounts.Where(a => _configuration.AccountNames.Contains(a.Name));
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(Budget budget, Account account)
        {
            var now = DateTime.Now;
            var date = _configuration.SinceDate.HasValue
                ? _configuration.SinceDate.Value
                : new DateTime(now.Year, now.Month, 1);

            var response = await _client.GetAsync(BASE_URL + "/budgets/" + budget.ID + "/accounts/" + account.ID + "/transactions?since_date=" + date.ToString("yyyy-MM-dd"));
            return await ParseResponseForModels<Transaction>(response);
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
            var json = JsonConvert.DeserializeObject<JObject>(responseString, Configuration.Instance.JSONSettings);
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
