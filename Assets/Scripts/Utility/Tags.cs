namespace Tags {
    struct FigureTags {
        public static string TeamA { get; } = "Selectable";
        public static string TeamB { get; } = "Selectable";
        public static string King { get; } = "King";
    }

    struct TileTags {
        public static string Accessible { get; } = "AT";
        public static string SpawnA { get; } = "Spawn_Team_A";
        public static string SpawnB { get; } = "Spawn_Team_B";
        public static string King { get; } = "KingTile";
        public static string Haven { get; } = "DeathTile";
        public static string Highlight { get; } = "Highlight";
    }
}
