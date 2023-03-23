namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Sql
{
    public class LocalConnectionPropertiesModel
    {
        /// <summary>
        /// The unique Id of this tree node
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// Indicates whether this connection is connected
        /// </summary>
        public bool Connected { get; set; }

        /// <summary>
        /// Indicates whether this container is expanded in the tree
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Indicates whether this connection is a favorite
        /// </summary>
        public bool Favorite { get; set; }
    }
}
