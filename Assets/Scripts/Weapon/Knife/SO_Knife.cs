using Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "New Knife", menuName = "Knife/Knife", order = 2)]
public class SO_Knife : ScriptableObject, IWeaponVisitor
{
    public string Name;
    public GunType Type;
    public int Damage;
    public float AttackRate;
    public AudioClip AudioClip;
    public Sprite Sprite;
    public GameObject ModelPrefab;
    public GameObject BloodParticle;
    public LayerMask HitMask;

    [Header("Spawn")]
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    public Vector3 PivotPoint;

    [Header("UI Icons")]
    public Sprite WeaponIcon;
    public Sprite BulletIcon;

    private MonoBehaviour _activeMonoBehaviour;
    private GameObject _model;
    private AudioSource _audioSource;
    private float _lastShootTime;

    private InputHandler _input;
    private CinemachineVirtualCamera _camera;
    private Animator _weaponAnim;

    public void Visit(EnemyHitBox enemy)
    {
        enemy.Health.TakeDamage(Damage, Vector3.zero, enemy.Type == EnemyHitBoxType.Head ? 1000 : 1);
    }

    public void Visit(SO_Gun gun)
    {
        throw new System.NotImplementedException();
    }

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this._activeMonoBehaviour = activeMonoBehaviour;
        _lastShootTime = 0;

        _model = Instantiate(ModelPrefab);
        _model.transform.SetParent(parent, false);
        _model.transform.localPosition = SpawnPoint;
        _model.transform.localRotation = Quaternion.Euler(SpawnRotation);
        _weaponAnim = _model.GetComponent<Animator>();
        _input = FindObjectOfType<InputHandler>();
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        _audioSource = _model.GetComponent<AudioSource>();
    }

    public void TryToAttack(PlayerUI playerUI)
    {
        if (Time.time > AttackRate + _lastShootTime)
        {
            _lastShootTime = Time.time;

            //PlayShootingClip(_audioSource);

            _weaponAnim.SetTrigger("Shoot");

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 shootDirection = ray.origin - _model.transform.forward;
            shootDirection.Normalize();

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, HitMask))
            {
                if (hit.collider.TryGetComponent(out HitBox hitbox))
                {
                    Instantiate(BloodParticle, hit.point, Quaternion.Euler(hit.point));

                    hitbox.Accept(this);
                    playerUI.CrosshairHit();
                }
            }
        }
    }

    public void PlayShootingClip(AudioSource source)
    {
        source.PlayOneShot(AudioClip);
    }
}
  

