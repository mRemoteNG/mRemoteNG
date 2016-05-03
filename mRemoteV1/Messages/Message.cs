using System;


namespace mRemoteNG.Messages
{
	public class Message
	{
		private MessageClass _MsgClass;
        private string _MsgText;
        private DateTime _MsgDate;

        public MessageClass MsgClass
		{
			get
			{
				return _MsgClass;
			}
			set
			{
				_MsgClass = value;
			}
		}
		
        public string MsgText
		{
			get
			{
				return _MsgText;
			}
			set
			{
				_MsgText = value;
			}
		}
		
        public DateTime MsgDate
		{
			get
			{
				return _MsgDate;
			}
			set
			{
				_MsgDate = value;
			}
		}


        public Message()
            : this(MessageClass.InformationMsg, "", DateTime.Now)
        {
        }

        public Message(MessageClass messageClass, string messageText, DateTime messageDate)
        {
            _MsgClass = messageClass;
            _MsgText = messageText;
            _MsgDate = messageDate;
        }
	}
}