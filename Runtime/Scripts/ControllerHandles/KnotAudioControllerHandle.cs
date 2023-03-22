namespace Knot.Audio
{
    public partial struct KnotAudioControllerHandle
    {
        public readonly KnotAudioControllerBase Controller;

        public bool IsValid => Controller != null;


        public KnotAudioControllerHandle(KnotAudioControllerBase controller)
        {
            Controller = controller;
        }
    }
}
