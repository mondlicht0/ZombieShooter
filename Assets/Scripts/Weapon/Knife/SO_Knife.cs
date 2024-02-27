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

    private MonoBehaviour _activeMonoBehaviour;
    private AudioSource _audioSource;
    private float _lastShootTime;

    private InputHandler _input;
    private CinemachineVirtualCamera _camera;
    public GameObject Model;
    public Animator WeaponAnim;

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

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);
        WeaponAnim = Model.GetComponent<Animator>();
        _input = FindObjectOfType<InputHandler>();
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        _audioSource = Model.GetComponent<AudioSource>();

        Model.SetActive(false);
    }

    public void TryToAttack(PlayerUI playerUI)
    {
        if (Time.time > AttackRate + _lastShootTime)
        {
            _lastShootTime = Time.time;

            //PlayShootingClip(_audioSource);

            WeaponAnim.SetTrigger("Attack-3");

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Vector3 shootDirection = ray.origin - Model.transform.forward;
            //shootDirection.Normalize();

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

    public void Visit(GameObject environmentObject)
    {
        
    }
}
  

