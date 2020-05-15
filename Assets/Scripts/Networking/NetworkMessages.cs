using Mirror;
using Tags;

namespace NetworkMessages {
    public class MoveMessage : MessageBase {
        public int originI;
        public int originJ;
        public int destI;
        public int destJ;
    }

    public class TeamMessage : MessageBase {
        public TeamTag teamTag;
    }
}
