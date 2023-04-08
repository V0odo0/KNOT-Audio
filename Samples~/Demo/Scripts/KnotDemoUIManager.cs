using UnityEngine;
using UnityEngine.UI;

namespace Knot.Audio.Demo
{
    public class KnotDemoUIManager : MonoBehaviour
    {
        public static KnotDemoUIManager Instance => _instance ?? (_instance = FindObjectOfType<KnotDemoUIManager>());
        private static KnotDemoUIManager _instance;


        [SerializeField] private Text _allTipsText;
        [SerializeField] private Text _tipsCollectedText;
        [SerializeField] private GameObject[] _tipPanels;


        [Header("Sounds")]
        [SerializeField] private KnotAudioDataReference _openTipPanelSound;


        void Awake()
        {
            _allTipsText.text = new string('•', _tipPanels.Length);
            foreach (var p in _tipPanels)
                p.gameObject.SetActive(false);
        }

        public void ShowTipPanel(int id)
        {
            if (id < 0 || id >= _tipPanels.Length)
                return;

            _tipPanels[id].gameObject.SetActive(true);
            KnotDemoGameManager.Instance.TipsCollected.Add(id);

            _tipsCollectedText.text = new string('•', KnotDemoGameManager.Instance.TipsCollected.Count);

            _openTipPanelSound.Play();
        }

        public void HideTipPanel(int id)
        {
            if (id < 0 || id >= _tipPanels.Length)
                return;

            _tipPanels[id].gameObject.SetActive(false);
        }
    }
}
