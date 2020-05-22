namespace Tags {
    public struct FigureTags {
        public static string TeamA { get; } = "playerA";
        public static string TeamB { get; } = "playerB";
        public static string King { get; } = "playerKing";
        public static string Captured { get; set; } = "playerCaptured";
    }

    public struct TileTags {
        public static string Accessible { get; } = "tileDefault";
        public static string SpawnA { get; } = "tileSpawnA";
        public static string SpawnB { get; } = "tileSpawnB";
        public static string King { get; } = "tileKing";
        public static string Haven { get; } = "tileHaven";
        public static string Highlight { get; } = "tileHighlight";
    }

    public enum TeamTag {
        Attackers,
        Defenders
    }
}
