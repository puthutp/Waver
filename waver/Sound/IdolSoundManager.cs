
namespace KaraokeIdol
{
    public class IdolSoundManager : SGateSoundManager
    {
        public IdolGame ParentGame;

        public IdolSoundManager(IdolGame _parentGame) :
            base()
        {
            ParentGame = _parentGame;
        }

        public void Load()
        {
            //for (int i = 0; i < IdolConstants.DUMMY_MUSICS_NAME.Length; i++)
            //{
            //    this.AddSoundStream(IdolConstants.DUMMY_MUSICS_NAME[i], IdolConstants.DUMMY_MUSICS_PATH[i]);
            //}
        }
    }
}
