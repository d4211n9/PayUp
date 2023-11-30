using api.models;

namespace service;

public class TransactionCalculator
{
    public IEnumerable<Transaction> CalculateTransActions(IEnumerable<BalanceDto> balances)
    {
        var payers = new Dictionary<int, decimal>();
        var payees = new Dictionary<int, decimal>();

        //filters users by amount into payers and payees
        foreach (var b in balances)
        {
            if (b.Amount < 0) {
                payers.Add(b.UserId, b.Amount);
            } else if (b.Amount > 0) {
                payees.Add(b.UserId, b.Amount);
            } 
        }
        return MakeTransactions(payers, payees);
    }

    private IEnumerable<Transaction> MakeTransactions(Dictionary<int, Decimal> payers, Dictionary<int, Decimal> payees)
    {
        var transactionList = new List<Transaction>(); 
        //loops through the payers and payees until all users are square
        while (payers.Count > 0 && payees.Count > 0)
        {
            var lowestPayerKey = payers.OrderBy(pair => pair.Value).FirstOrDefault().Key;
            var highestPayeeKey = payees.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;

            //gets the amounts for both payer and payee
            decimal payerAmount = payers[lowestPayerKey];
            decimal payeeAmount = payees[highestPayeeKey];
            //gets the amount that has to payed before one part is square
            decimal settledAmount = Math.Min(Math.Abs(payerAmount), payeeAmount);

            // Perform the settlement
            // Update payer's amount
            payers[lowestPayerKey] += settledAmount;
            // Update payee's amount
            payees[highestPayeeKey] -= settledAmount;

            //creates a transAction Object, to keep track of transactions
            var transAction = new Transaction
            {
                PayeeId = highestPayeeKey,
                PayerId = lowestPayerKey,
                Amount = settledAmount
            };
            transactionList.Add(transAction);

            // Remove payer or payee if the amount becomes zero
            if (payers[lowestPayerKey] == 0) payers.Remove(lowestPayerKey);
            if (payees[highestPayeeKey] == 0) payees.Remove(highestPayeeKey);
        }
        return transactionList;
    }
}