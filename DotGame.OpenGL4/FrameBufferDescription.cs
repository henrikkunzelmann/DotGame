namespace DotGame.OpenGL4
{
    internal struct FrameBufferDescription
    {
        internal int[] ColorAttachmentIDs { get; private set; }
        internal int DepthAttachmentID { get; private set; }

        internal bool HasAttachments
        {
            get { return !(DepthAttachmentID <= 0 && (ColorAttachmentIDs == null || ColorAttachmentIDs.Length == 0)); }
        }

        
        internal FrameBufferDescription(params int[] colorAttachmentIDs) : this()
        {
            ColorAttachmentIDs = colorAttachmentIDs;
        }
        internal FrameBufferDescription(int depthAttachmentID, params int[] colorAttachmentIDs)
            : this()
        {
            DepthAttachmentID = depthAttachmentID;
            ColorAttachmentIDs = colorAttachmentIDs;
        }

        public override int GetHashCode()
        {/*
            string colorHash = "";
            if (ColorAttachmentIDs == null || ColorAttachmentIDs.Length == 0)
                colorHash = "-1";
            else
                foreach (int color in ColorAttachmentIDs)
                    colorHash += color.ToString();

            return int.Parse(DepthAttachmentID.ToString() + colorHash);
            */
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + DepthAttachmentID;
                if (ColorAttachmentIDs == null || ColorAttachmentIDs.Length == 0)
                {
                    hash = hash * 23 + 0;
                }
                else
                {
                    hash = hash * 23 + ColorAttachmentIDs.Length;
                    foreach (int color in ColorAttachmentIDs)
                        hash = hash * 23 + color;
                }
                return hash;
            }
        }

        public bool Equals(FrameBufferDescription desc1)
        {
            return desc1.GetHashCode() == GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is FrameBufferDescription)
                return obj.GetHashCode() == obj.GetHashCode();

            return base.Equals(obj);
        }

        public static bool operator ==(FrameBufferDescription desc1, FrameBufferDescription desc2)
        {
            return desc1.Equals(desc2);
        }
        public static bool operator !=(FrameBufferDescription desc1, FrameBufferDescription desc2)
        {
            return !desc1.Equals(desc2);
        }
    }
}
