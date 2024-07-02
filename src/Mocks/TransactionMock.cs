using RestApiSample.Models;

namespace RestApiSample.Mocks
{

    public class TransactionMock : BaseMock<Transaction>
    {

        public TransactionMock()
        {
            _value = new Transaction
            {
                TotalAmount = 2,
                TotalPrice = 25500,
                CreatedBy = "admin"
            };
        }
    }
}

