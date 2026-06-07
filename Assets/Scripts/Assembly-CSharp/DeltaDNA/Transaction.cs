namespace DeltaDNA
{
	public class Transaction<T> : GameEvent<T> where T : Transaction<T>
	{
		public Transaction(string name, string type, Product productsReceived, Product productsSpent)
			: base((string)null)
		{
		}

		public T SetTransactionId(string transactionId)
		{
			return null;
		}

		public T SetReceipt(string receipt)
		{
			return null;
		}

		public T SetReceiptSignature(string receiptSignature)
		{
			return null;
		}

		public T SetServer(string server)
		{
			return null;
		}

		public T SetTransactorId(string transactorId)
		{
			return null;
		}

		public T SetProductId(string productId)
		{
			return null;
		}
	}
	public class Transaction : Transaction<Transaction>
	{
		public Transaction(string name, string type, Product productsReceived, Product productsSpent)
			: base((string)null, (string)null, (Product)null, (Product)null)
		{
		}
	}
}
