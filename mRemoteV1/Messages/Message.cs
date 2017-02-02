using System;


namespace mRemoteNG.Messages
{
	public class Message : IMessage
	{
	    public MessageClass Class { get; set; }
	    public string Text { get; set; }
	    public DateTime Date { get; set; }
	    public bool OnlyLog { get; set; }


	    public Message()
            : this(MessageClass.InformationMsg, "", DateTime.Now, false)
        {
        }

        public Message(MessageClass messageClass, string messageText, DateTime messageDate, bool onlyLog)
        {
            Class = messageClass;
            Text = messageText;
            Date = messageDate;
            OnlyLog = onlyLog;
        }
	}
}