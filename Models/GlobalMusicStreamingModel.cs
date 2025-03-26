using CsvHelper.Configuration.Attributes;

class GlobalMusicStreamingModel {
    [Name("User_Id")]
    public int UserId { get; set; }

    [Name("Age")]
    public int Age { get; set; }

    [Name("Country")]
    public string? Country { get; set; }
    
    [Name("Streaming Platform")]
    public string? StreamingPlatform { get; set; }

    [Name("Top Genre")]
    public string? TopGenre { get; set; }

    [Name("Minutes Streamed Per Day")]
    public string? MinutesStreamedPerDay { get; set; }

    [Name("Number of Songs Liked")]
    public int NumberOfSongsLiked { get; set; }

    [Name("Most Played Artist")]
    public string? MostPlayedArtist { get; set; }

    [Name("Subscription Type")]
    public string? SubscriptionType { get; set; }

    [Name("Listening Time")]
    public string? ListeningTime { get; set; }

    [Name("Discover Weekly Engagement")]
    public double DiscoverWeeklyEngagement { get; set; }

    [Name("Repeat Song Rate")]
    public double RepeatSongRate { get; set; }
}