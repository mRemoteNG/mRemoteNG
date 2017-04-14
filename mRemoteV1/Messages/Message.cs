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
            : this(MessageClass.InformationMsg, "")
        {
        }

        public Message(MessageClass messageClass, string messageText, bool onlyLog = false)
        {
            Class = messageClass;
            Text = messageText;
            Date = DateTime.Now;
            OnlyLog = onlyLog;
        }
	}
}