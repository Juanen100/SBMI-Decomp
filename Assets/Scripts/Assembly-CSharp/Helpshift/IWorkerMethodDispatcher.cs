namespace Helpshift
{
	public interface IWorkerMethodDispatcher
	{
		void resolveAndCallApi(string methodIdentifier, string api, object[] args);
	}
}
