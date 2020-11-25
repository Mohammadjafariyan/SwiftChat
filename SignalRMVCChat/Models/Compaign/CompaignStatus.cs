namespace SignalRMVCChat.Models.Compaign
{
    public enum CompaignStatus
    {
        ReadyToActivate = 1,
        ReadyToSend = 2,
        NotConfigured = 3,
        Sending = 4,
        Stopped = 5,
        Sent = 6
    }
}