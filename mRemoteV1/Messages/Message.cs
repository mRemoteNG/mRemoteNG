using System;


namespace mRemoteNG.Messages
{
	public class Message : IMessage
	{
	    public MessageClass Class { get; set; }

	    public string Text { get; set; }

	    public DateTime Date { get; set; }


	    public Message()
            : this(MessageClass.InformationMsg, "", DateTime.Now)
        {
        }

        public Message(MessageClass messageClass, string messageText, DateTime messageDate)
        {
            Class = messageClass;
            Text = messageText;
            Date = messageDate;
        }
	}
}