using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTheVillage : MonoBehaviour
{
    // Panels
    [SerializeField] private GameObject _loosePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _pausePanel;
    // Buttons
    [SerializeField] private Button _pauseButton;
    // Text
    [SerializeField] private Text _fishText;
    [SerializeField] private Text _fisherText;
    [SerializeField] private Text _warriorText;
    [SerializeField] private Text _mousetrapCountText;
    [SerializeField] private Text _raidText;
    [SerializeField] private Text _soundOff;
    // Image
    [SerializeField] private Image _fishingTimerImg;
    [SerializeField] private Image _eatTimerImg;
    [SerializeField] private Image _raidTimerImg;
    [SerializeField] private Image _pauseImg;

    [SerializeField] private Image _fisherTimerImg;
    [SerializeField] private Image _warriorTimerImg;
    // Sound
    /*[SerializeField] private AudioListener _audioListener;
    [SerializeField] private AudioSource _buttonClickSound;
    [SerializeField] private AudioSource _eatingSound;
    [SerializeField] private AudioSource _fishSound;
    [SerializeField] private AudioSource _looseSound;
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _raidSound;
    [SerializeField] private AudioSource _mousetrapSound;
    [SerializeField] private AudioSource _fisherSound;
    [SerializeField] private AudioSource _warriorSound;*/
    // Флаги
    private bool _isFisherPressed;
    private bool _isWarriorPressed;
    private bool _isFisherPurchased;
    private bool _isWarriorPurchased;
    private bool _isGamePaused;
    // Счётчики
    private int _fisherCount = 1;
    private int _warriorCount;
    private int _fishCount;
    private int _mouseCount = 2;
    private int _mousetrapCount = 3;
    // Для статистики
    public static SaveTheVillage Instance { get; private set; }
    public int fishProduced;
    public int warriorsProduced;
    public int fishersProduced;
    public int repelledRaids;
    // Время в таймерах
    private float _fishingTime;
    private float _eatTime = -5;
    private float _raidTime;
    private float _fisherTime;
    private float _warriorTime;
    // Настройка стоимости найма рыбака и воина
    [SerializeField] private int _fishToHire = 2;

    /// <summary>
    /// Обновление настраиваемого таймера
    /// </summary>
    /// <param name="index">Индекс таймера</param>
    /// <param name="maxTime">Время (в секундах)</param>
    private void ChangeTimer(int index, int maxTime)
    {
        switch (index)
        {
            case 0: // Пшеница
                if (_fishingTime >= maxTime)
                {
                    _fishCount += _fisherCount * 2;
                    fishProduced += _fisherCount * 2;
                    _fishText.text = $"{_fishCount}";
                    //_fishSound.Play();

                    if (_fishCount > 500) // Выигрыш
                    {
                        Time.timeScale = 0;
                        _winPanel.SetActive(true);
                        _pauseButton.enabled = false;
                        //_winSound.Play();
                    }
                    _fishingTime = 0;
                }
                _fishText.text = $"{_fishCount}";
                _fishingTimerImg.fillAmount = _fishingTime / maxTime;
                _fishingTime += Time.deltaTime;
                break;
            case 1: // Голод
                if (_eatTime == -5)  
                {
                    _eatTime = maxTime; // поле _eatTime должно определяться maxTime (т.к. заполняется сверху вниз)
                }
                if (_warriorCount > 0) // Проверка наличия воинов
                {
                    if (_eatTime <= 0)
                    {
                        _fishCount -= _warriorCount;
                        if (_fishCount < 0) // Проигрыш из-за голода
                        {
                            Time.timeScale = 0;
                            _loosePanel.SetActive(true);
                            _pauseButton.enabled = false;
                            //_looseSound.Play();
                        }
                        _fishText.text = $"{_fishCount}";
                        _eatTime = maxTime;
                        //_eatingSound.Play();
                    }
                    _eatTime -= Time.deltaTime;
                }
                else
                {
                    _eatTime = maxTime;
                }
                _eatTimerImg.fillAmount = _eatTime / maxTime;
                break;
            case 2: // Рейд
                maxTime += repelledRaids * 2;  // Увеличение времени рейда в зависимости от пройденных волн
                _mousetrapCountText.text = $"{_mousetrapCount}";
                if (_raidTime >= maxTime)
                {
                    if (_mousetrapCount == 0) // Проверка наличия мышеловок
                    {
                        //_raidSound.Play();
                        _warriorCount -= _mouseCount;
                    }
                    else
                    {
                        //_mousetrapSound.Play();
                        _mousetrapCount--;
                        _mousetrapCountText.text = $"{_mousetrapCount}";
                    }                     
                    if (_warriorCount < 0) // Проигрыш
                    {
                        Time.timeScale = 0;
                        _loosePanel.SetActive(true);
                        _pauseButton.enabled = false;
                        //_looseSound.Play();
                    }
                    repelledRaids++;
                    if (repelledRaids == 10) // Выигрыш
                    {
                        Time.timeScale = 0;
                        _winPanel.SetActive(true);
                        _pauseButton.enabled = false;
                        //_winSound.Play();
                    }
                    _warriorText.text = $"{_warriorCount}";
                    _mouseCount = (int)Mathf.Round(_mouseCount * 1.5f);
                    _raidTime = 0;
                }
                _raidText.text = $"{_mouseCount}";
                _raidTimerImg.fillAmount = _raidTime / maxTime;
                _raidTime += Time.deltaTime;
                break;
            case 3: // Найм рыбака
                _fisherText.text = $"{_fisherCount}";
                if ((_isFisherPressed && _fishCount >= _fishToHire) || _isFisherPurchased)
                {
                    if (!_isFisherPurchased) // Проверка покупки рыбака
                    {
                        _fishCount -= _fishToHire;
                        _isFisherPurchased = true;
                    }
                    if (_fisherTime >= maxTime) // Добавление рыбака
                    {
                        //_fisherSound.Play();
                        _fisherCount++;
                        fishersProduced++;
                        _fisherText.text = $"{_fisherCount}";

                        if (_fisherCount == 30) // Выигрыш
                        {
                            Time.timeScale = 0;
                            _winPanel.SetActive(true);
                            _pauseButton.enabled = false;
                            //_winSound.Play();
                        }
                        _fisherTime = 0;
                        _isFisherPressed = false;
                        _isFisherPurchased = false;
                    }
                    _fisherTimerImg.fillAmount = _fisherTime / maxTime;
                    _fisherTime += Time.deltaTime;
                }
                else
                {
                    _isFisherPressed = false;
                }
                break;
            case 4: // Найм воина
                _warriorText.text = $"{_warriorCount}";
                if ((_isWarriorPressed && _fishCount >= _fishToHire) || _isWarriorPurchased)
                {
                    if (!_isWarriorPurchased) // Проверка покупки воина
                    {
                        _fishCount -= _fishToHire;
                        _isWarriorPurchased = true;
                    }
                    if (_warriorTime >= maxTime) // Добавление воина
                    {
                        //_warriorSound.Play();
                        _warriorCount++;
                        warriorsProduced++;
                        _warriorText.text = $"{_warriorCount}";

                        _warriorTime = 0;
                        _isWarriorPressed = false;
                        _isWarriorPurchased = false;
                    }
                    _warriorTimerImg.fillAmount = _warriorTime / maxTime;
                    _warriorTime += Time.deltaTime;
                }
                else
                {
                    _isWarriorPressed = false;
                }
                break;
            default:
                break;
        }
    }
    public void Awake()
    {
        Instance = this;
    }
    public void OnClickPause() // Пауза
    {
        if (!_isGamePaused)
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);

        }
        else
        {
            Time.timeScale = 1;
            _pausePanel.SetActive(false);
        }
        _isGamePaused = !_isGamePaused;
        //_buttonClickSound.Play();
    }
    /*public void OnClickSoundChange() // вкл/выкл звук
    {
        _audioListener.enabled = !_audioListener.enabled;
        if (!_audioListener.enabled)
        {
            _soundOff.text = "/";
        }
        else
        {
            _soundOff.text = "";
        }
    }*/
    public void HireVillager() // Найм рыбака (по кнопке)
    {
        //_buttonClickSound.Play();
        _isFisherPressed = true;
    }
    public void HireWarrior() // Найм воина (по кнопке)
    {
        //_buttonClickSound.Play();
        _isWarriorPressed = true;
    }
    public void OnClickRestart() // Рестарт
    {
        _pauseButton.enabled = true;
        _loosePanel.SetActive(false);
        _winPanel.SetActive(false);
        _fisherCount = 1;
        _warriorCount = 0;
        _fishCount = 0;
        _mouseCount = 2;
        _mousetrapCount = 3;

        _fishingTime = 0;
        _eatTime = -5;
        _raidTime = 0;
        _fisherTime = 0;
        _warriorTime = 0;
        repelledRaids = 0;

        _fisherTimerImg.fillAmount = 0;
        _warriorTimerImg.fillAmount = 0;

        _isFisherPressed = false;
        _isWarriorPressed = false;
        _isFisherPurchased = false;
        _isWarriorPurchased = false;

        Time.timeScale = 1;

        //_buttonClickSound.Play();
    }
    void Update()
    {
        ChangeTimer(0, 5); // 0 Рыба
        ChangeTimer(1, 7); // 1 Голод
        ChangeTimer(2, 22);// 2 Рейд *(значение времени у рейда не статично, оно будет увеличиваться)
        ChangeTimer(3, 4); // 3 Найм рыбака
        ChangeTimer(4, 4); // 4 Найм воина
    }
}
