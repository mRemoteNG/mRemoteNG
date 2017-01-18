using System;


namespace mRemoteNG.Messages
{
	public class Message
	{
	    public MessageClass MsgClass { get; set; }

	    public string MsgText { get; set; }

	    public DateTime MsgDate { get; set; }


	    public Message()
            : this(MessageClass.InformationMsg, "", DateTime.Now)
        {
        }

        public Message(MessageClass messageClass, string messageText, DateTime messageDate)
        {
            MsgClass = messageClass;
            MsgText = messageText;
            MsgDate = messageDate;
        }
	}
}