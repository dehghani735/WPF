using System;

namespace WpfApp3
{
    // 980808 inspired by https://stackoverflow.com/questions/2578615/c-sharp-event-handlers-using-an-enum
    public class TransactionEventArgs : EventArgs
    {
        public enum eStatus
        {
            Started,
            Processing,
            Passed,
            Failed
        }

        public eStatus EventType { get; set; }
        public TransactionConfig EventMessage { get; set; }
        public string Message { get; set; }

        public TransactionEventArgs(eStatus eventType, TransactionConfig eventMessage)
        {
            this.EventType = eventType;
            this.EventMessage = eventMessage;
        }

        public TransactionEventArgs(eStatus eventType, TransactionConfig eventMessage, string message)
        {
            this.EventType = eventType;
            this.EventMessage = eventMessage;
            this.Message = message;
        }
    }

    public abstract class Transaction //: IStatus // TODO: as Receiver i think
    {
        private TransactionConfig _transactionConfig;

        public event EventHandler<TransactionEventArgs> StatusChanged;

        public Transaction(TransactionConfig transactionConfig)
        {
            TransactionConfig = transactionConfig;
        }

        public Transaction()
        {
        }

        public TransactionConfig TransactionConfig
        {
            get { return _transactionConfig; }
            set { _transactionConfig = value; }
        }

        public abstract void Ali();

        public abstract string Process(Network network);

        protected virtual void OnStatusStarting(TransactionConfig transactionConfig)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this,
                    new TransactionEventArgs(TransactionEventArgs.eStatus.Started, transactionConfig));
                // for example
            }
        }

        protected virtual void OnStatusProcessing(TransactionConfig transactionConfig)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this,
                    new TransactionEventArgs(TransactionEventArgs.eStatus.Processing, transactionConfig));
                // for example
            }
        }

        protected virtual void OnStatusPassed(TransactionConfig transactionConfig)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this,
                    new TransactionEventArgs(TransactionEventArgs.eStatus.Passed, transactionConfig));
                // for example
            }
        }

        protected virtual void OnStatusFailed(TransactionConfig transactionConfig, string message)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this,
                    new TransactionEventArgs(TransactionEventArgs.eStatus.Failed, transactionConfig, message));
                // for example
            }
        }
    }
}