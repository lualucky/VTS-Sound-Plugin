using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Randomize : StateMachineBehaviour
{
    public bool Parent;

    public bool Child;
    public List<int> ChildHierarchy;

    public bool FlipX;
    public bool FlipY;
    public List<Color> Colors;

    public bool UserColors;
    public bool UserFlip;

    public bool HorizontalPosition;
    public bool VerticalPosition;

    public bool RandomAngularVelocity;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject obj = animator.gameObject;

        SpriteRenderer renderer;
        Image uirenderer;

        if (Parent)
            obj = animator.gameObject.transform.parent.gameObject;
        else if(Child)
        {
            foreach(int i in ChildHierarchy)
            {
                obj = obj.transform.GetChild(i).gameObject;
            }
        }

        renderer = obj.GetComponent<SpriteRenderer>();
        uirenderer = obj.GetComponent<Image>();

        if (!renderer)
        {
            if (FlipX)
            {
                Vector3 v = obj.transform.localScale;
                obj.transform.localScale = new Vector3(-v.x, v.y, v.z);
            }
        }
        if (FlipY || UserColors || Colors.Count > 0)
        {
            if (renderer)
            {
                if (FlipX)
                    renderer.flipX = Random.value <= .5f;
                if (FlipY)
                    renderer.flipY = Random.value <= .5f;
                if (Colors.Count > 0)
                {
                    renderer.color = Colors[Random.Range(0, Colors.Count)];
                }
            }
            else if (uirenderer)
            {
                if (Colors.Count > 0)
                {
                    uirenderer.color = Colors[Random.Range(0, Colors.Count)];
                }
            }
            else
                Debug.LogError("No sprite found on " + obj.name);
        }

        if (HorizontalPosition)
        {
            if (obj.GetComponent<RectTransform>())
            {
                RectTransform trans = obj.GetComponent<RectTransform>();
                Vector3 pos = Camera.main.ScreenToWorldPoint(trans.position);
                trans.Translate(ScreenI.RandomWidth(), Space.World);
            }
            else
            {
                Vector3 pos = obj.transform.position;
                obj.transform.position = ScreenI.RandomWidth() + new Vector3(.25f, pos.y, pos.z);
            }
        }
        if (VerticalPosition)
        {
            Vector3 pos = obj.transform.position;
            obj.transform.position = ScreenI.RandomHeight() + new Vector3(pos.x - .25f, 0, pos.z);
        }

        Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
        if(body != null)
        {
            if(RandomAngularVelocity)
            {
                body.angularVelocity = Random.Range(-100f, 100f);
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
