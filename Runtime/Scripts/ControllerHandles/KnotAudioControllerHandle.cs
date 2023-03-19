namespace Knot.Audio
{
    public partial struct KnotAudioControllerHandle
    {
        public readonly KnotAudioController Controller;

        public bool IsValid => Controller != null;


        public KnotAudioControllerHandle(KnotAudioController controller)
        {
            Controller = controller;
        }
    }
}
