using mRemoteNG.Tools;

namespace mRemoteNG.Connection.Protocol.RDP
{
    public enum RDPSounds
    {
        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDPSoundBringToThisComputer))]
        BringToThisComputer = 0,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.RDPSoundLeaveAtRemoteComputer))]
        LeaveAtRemoteComputer = 1,

        [LocalizedAttributes.LocalizedDescription(nameof(Language.DoNotPlay))]
        DoNotPlay = 2
    }
}