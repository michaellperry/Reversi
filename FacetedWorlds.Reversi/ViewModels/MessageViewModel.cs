using FacetedWorlds.Reversi.Model;

namespace FacetedWorlds.Reversi.ViewModels
{
    public class MessageViewModel
    {
        private Message _message;

        public MessageViewModel(Message message)
        {
            _message = message;
        }

        public string Sender
        {
            get { return _message.Sender.User.UserName; }
        }

        public string Body
        {
            get { return _message.Body; }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            MessageViewModel that = obj as MessageViewModel;
            if (that == null)
                return false;
            return this._message == that._message;
        }

        public override int GetHashCode()
        {
            return _message.GetHashCode();
        }
    }
}
