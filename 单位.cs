using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class ��λ����
{
    public string ����;            // ��λ���� 
    public string ����Ŀ���ǩ; // Ҫ������Ŀ���ǩ������ "�췽" �� "����" 
    public float ������;          // ������ 
    public float ��󹥻���;      // ��󹥻��� 
    public float ����;            // ���� 
    public float ������;        // ������ 
    public float �ƶ��ٶ�;        // �ƶ��ٶ� 
    public float ����ƶ��ٶ�;    // ����ƶ��ٶ� 
    public float ����ֵ;          // ��ǰ����ֵ 
    public float �������ֵ;      // �������ֵ 
    public float ħ��ֵ;          // ��ǰħ��ֵ 
    public float ���ħ��ֵ;      // ���ħ��ֵ 
    public float �ظ�Ѫ���ٶ�;    // ÿ��ظ�����ֵ 
    public float ���ظ�Ѫ���ٶ�;// ��������ظ��ٶ� 
    public float �ظ�ħ���ٶ�;    // ÿ��ظ�ħ��ֵ 
    public float ���ظ�ħ���ٶ�;// ���ħ���ظ��ٶ� 
    public float ��������;        // �������� 
    public float ��󹥻�����;    // ��󹥻����� 
    public float �����ٶ�;        // �����ٶ� 
    public float ��󹥻��ٶ�;    // ��󹥻��ٶ� 
    public float ������Χ;        // ������Χ���������⹥���� 
    public float ��󹥻���Χ;    // ��󹥻���Χ 
    public ��λ����(string ����, float ������, float ��󹥻���, float ����, float ������,
        float �ƶ��ٶ�, float ����ƶ��ٶ�, float ����ֵ, float �������ֵ, float ħ��ֵ,
        float ���ħ��ֵ, float �ظ�Ѫ���ٶ�, float ���ظ�Ѫ���ٶ�, float �ظ�ħ���ٶ�,
        float ���ظ�ħ���ٶ�, float ��������, float ��󹥻�����, float �����ٶ�,
        float ��󹥻��ٶ�, float ������Χ = 0, float ��󹥻���Χ = 0)
    {
        this.���� = ����;
        this.������ = Mathf.Min(������, ��󹥻���);
        this.��󹥻��� = ��󹥻���;
        this.���� = Mathf.Min(����, ������);
        this.������ = ������;
        this.�ƶ��ٶ� = Mathf.Min(�ƶ��ٶ�, ����ƶ��ٶ�);
        this.����ƶ��ٶ� = ����ƶ��ٶ�;
        this.����ֵ = Mathf.Min(����ֵ, �������ֵ);
        this.�������ֵ = �������ֵ;
        this.ħ��ֵ = Mathf.Min(ħ��ֵ, ���ħ��ֵ);
        this.���ħ��ֵ = ���ħ��ֵ;
        this.�ظ�Ѫ���ٶ� = Mathf.Min(�ظ�Ѫ���ٶ�, ���ظ�Ѫ���ٶ�);
        this.���ظ�Ѫ���ٶ� = ���ظ�Ѫ���ٶ�;
        this.�ظ�ħ���ٶ� = Mathf.Min(�ظ�ħ���ٶ�, ���ظ�ħ���ٶ�);
        this.���ظ�ħ���ٶ� = ���ظ�ħ���ٶ�;
        this.�������� = Mathf.Min(��������, ��󹥻�����);
        this.��󹥻����� = ��󹥻�����;
        this.�����ٶ� = Mathf.Min(�����ٶ�, ��󹥻��ٶ�);
        this.��󹥻��ٶ� = ��󹥻��ٶ�;
        this.������Χ = Mathf.Min(������Χ, ��󹥻���Χ);
        this.��󹥻���Χ = ��󹥻���Χ;
    }

}

public abstract class ��λ : MonoBehaviour
{
    public ��λ���� ����;       // ��λ���� 
    public Animator ����������; // Animator�����ڿ��ƶ��� 
    public Transform ��ǰĿ��;   // ��ǰĿ�� 
    public float ������Χ = 10f; // �������˵ķ�Χ 
    protected Vector3 �ƶ�Ŀ��λ��; // ���ڴ洢�ƶ�Ŀ��λ��
    protected NavMeshAgent ��������; // ������� 

    protected float ��һ�ι���ʱ�� = 0f; // ���ƹ�������ļ�ʱ�� 
    protected float ����ʧ�ܳ���ʱ�� = 0f; // ����ʧ�ܵ��ۼ�ʱ�� 
    protected const float ����ʧ�����ʱ�� = 0.1f;

    public Image Ѫ��;

    protected virtual void Start()
    {
        if (���� == null || ���������� == null)
        {
            Debug.LogError("���Ի򶯻�������δ���ã�");
            return;
        }

        �������� = GetComponent<NavMeshAgent>();
        if (�������� == null)
        {
            Debug.LogError("ȱ�� NavMeshAgent �����");
            return;
        }

        ��������.speed = ����.�ƶ��ٶ�;
        ��������.stoppingDistance = ����.��������;
        ��ʼ��(����, ����������);
    }

    protected virtual void Update()
    {
        ����������.speed = ����.�����ٶ�;
        �Զ�Ѱ�Ҳ�����Ŀ��();
        �Զ��ظ�();
    }

    public void ��ʼ��(��λ���� ����, Animator ����������)
    {
        this.���� = ����;
        this.���������� = ����������;

        // ��������������Ϊ���ֵ

        ����.������ = ����.��󹥻���;
        ����.���� = ����.������;
        ����.�ƶ��ٶ� = ����.����ƶ��ٶ�;
        ����.����ֵ = ����.�������ֵ;
        ����.ħ��ֵ = ����.���ħ��ֵ;
        ����.�ظ�Ѫ���ٶ� = ����.���ظ�Ѫ���ٶ�;
        ����.�ظ�ħ���ٶ� = ����.���ظ�ħ���ٶ�;
        ����.�������� = ����.��󹥻�����;
        ����.�����ٶ� = ����.��󹥻��ٶ�;
        ����.������Χ = ����.��󹥻���Χ;

        // ����е�������NavMeshAgent���������ƶ��ٶ�
        if (�������� != null)
        {
            ��������.speed = ����.�ƶ��ٶ�;
        }
        Ѫ������();
    }


    public void �Զ��ظ�()
    {
        ����.����ֵ = Mathf.Min(����.����ֵ + ����.�ظ�Ѫ���ٶ� * Time.deltaTime, ����.�������ֵ);
        ����.ħ��ֵ = Mathf.Min(����.ħ��ֵ + ����.�ظ�ħ���ٶ� * Time.deltaTime, ����.���ħ��ֵ);
        Ѫ��.fillAmount = ����.����ֵ / ����.�������ֵ;
    }

    protected virtual void �Զ�Ѱ�Ҳ�����Ŀ��()
    {
        if (��ǰĿ�� != null)
        {
            // ����Ŀ���뵥λ֮��ľ���
            float ���� = Vector3.Distance(transform.position, ��ǰĿ��.position);

            // ���Ŀ���ڹ�����Χ�ڣ����й���
            if (���� <= ����.��������)
            {
                ����ʧ�ܳ���ʱ�� = 0f; // ���ù���ʧ��ʱ�� 
                if (Time.time >= ��һ�ι���ʱ��)
                {
                    ����(��ǰĿ��.gameObject);
                    ��һ�ι���ʱ�� = Time.time + 1f / ����.�����ٶ�;
                }
            }
            else
            {
                // ���Ŀ�곬���˹������룬ֹͣ����������Ѱ��Ŀ��
                ����ʧ�ܳ���ʱ�� += Time.deltaTime;

                // �������ʧ�ܵ�ʱ�䳬��������ƣ�����Ѱ��Ŀ��
                if (����ʧ�ܳ���ʱ�� > ����ʧ�����ʱ��)
                {
                    Debug.Log($"{����.����} ��������Ŀ�꣬����Ѱ����Ŀ��...");
                    ��ǰĿ�� = null; // ���õ�ǰĿ�� 
                    ����ʧ�ܳ���ʱ�� = 0f; // ���ü�ʱ�� 
                    Ѱ�ҵ���(����.����Ŀ���ǩ); // ����Ѱ��Ŀ�� 
                }
                else
                {
                    // ���Ŀ�껹�ڹ�����Χ�⣬�����ƶ�
                    �ƶ�(��ǰĿ��.position);
                }
            }
        }
        else
        {
            Ѱ�ҵ���(����.����Ŀ���ǩ); // ���û��Ŀ�꣬Ѱ���µ�Ŀ�� 
            if (��ǰĿ�� != null)
            {
                // �����ҵ�Ŀ��󣬿�ʼ�ƶ�
                �ƶ�(��ǰĿ��.position);
            }
        }

    }

    protected virtual void Ѱ�ҵ���(string �з���ǩ)
    {
        Collider[] ������ = Physics.OverlapSphere(transform.position, ������Χ);
        Transform ������� = null;
        float ��С���� = float.MaxValue;

        foreach (var ���� in ������)
        {
            if (����.CompareTag(�з���ǩ))
            {
                float ���� = Vector3.Distance(transform.position, ����.transform.position);
                if (���� < ��С����)
                {
                    ��С���� = ����;
                    ������� = ����.transform;
                }
            }
        }

        if (������� != null)
        {
            ��ǰĿ�� = �������;
        }
    }

    public virtual void ����(GameObject Ŀ��)
    {
        if (Ŀ�� == null) return;
        ��������.ResetPath(); // �����ǰ��·����ֹͣ����
        ��������.speed = 0f; // ֹͣ�ƶ�
        ��������.updatePosition = false; // ��ֹλ�ø���  ��������.ResetPath();

        Vector3 Ŀ�귽�� = Ŀ��.transform.position - transform.position;
        Ŀ�귽��.y = 0;
        transform.rotation = Quaternion.LookRotation(Ŀ�귽��); // ����Ŀ��
        if (���������� != null)
        {
            ����������.SetBool("����", true);
           
        }
        ��λ �з���λ = Ŀ��.GetComponent<��λ>();
        if (�з���λ != null)
        {
            float �˺� = Mathf.Max(����.������ - �з���λ.����.����, 0);
            �з���λ.����(�˺�, ����);
        }
    }

    public void Ѫ������()
    {


        Ѫ��.fillAmount = ����.����ֵ / ����.�������ֵ;
    }
    public virtual void ����(float �˺�ֵ, ��λ���� ����������)
    {
        ����.����ֵ -= �˺�ֵ;
        Ѫ������();
        if (���������� != null)
        {
            Debug.Log($"{����.����} �� {����������.����} ��������� {�˺�ֵ} ���˺���");
        }
        else
        {
            Debug.Log($"{����.����} �ܵ��� {�˺�ֵ} ���˺���");
        }
        if (����.����ֵ <= 0) ����(����������);
    }

    public virtual void ����(��λ���� ��������)
    {
        Debug.Log($"{����.����} ��������");
        Destroy(gameObject);
    }

    public virtual void �ƶ�(Vector3 Ŀ��λ��)
    {
        ����������.SetBool("����", false);
        ��������.updatePosition = true; // ��ֹλ�ø���  ��������.ResetPath();
        if (�������� != null)
        {
            �ƶ�Ŀ��λ�� = Ŀ��λ��; // �����ƶ�Ŀ��λ��
            ��������.speed = ����.�ƶ��ٶ�;
            ��������.SetDestination(Ŀ��λ��);
        }
    }
}
