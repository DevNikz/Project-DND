using UnityEngine;

public class SwipeReceiver : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    [SerializeField] private Vector3 _targetPosition = Vector3.zero;

    [SerializeField] public int _type = 0;

    private void Start() {
        GestureManager.Instance.OnSwipe += this.OnSwipe;
    }

    private void OnEnable() {
        this._targetPosition = this.transform.position;
    }

    private void OnDisable() {
        GestureManager.Instance.OnSwipe -= this.OnSwipe;
    }

    public void OnSwipe(object sender, SwipeEventArgs args) {
        switch(this._type) {
            case 0:
                this.MovePerpendicular(args);
                break;
            case 1:
                this.MoveDiagonal(args);
                break;
        }
    }

    public void MovePerpendicular(SwipeEventArgs args) {
        // Vector3 direction = Vector3.zero;
        Vector3 direction = new Vector3(args.Direction.x, args.Direction.y, 0f);
        switch(args.SwipeDirection) {
            case ESwipeDirection.Up:
                Debug.Log("Up");
                direction = Vector3.up;
                // direction.y = 1;
                break;
            case ESwipeDirection.Down:
                Debug.Log("Down");
                direction = Vector3.down;
                // direction.y = -1;
                break;
            case ESwipeDirection.Left:
                Debug.Log("Left");
                direction = Vector3.left;
                // direction.x = -1;
                break;
            case ESwipeDirection.Right:
                Debug.Log("Right");
                direction = Vector3.right;
                // direction.x = 1;
                break;
        }
        this._targetPosition += direction * 5;
    }

    public void MoveDiagonal(SwipeEventArgs args) {
        Vector3 direction = new Vector3(args.Direction.x, args.Direction.y, 0f);
        switch(args.SwipeDirection) {
            case ESwipeDirection.UpLeft:
                Debug.Log("Up");
                direction = Vector3.up + Vector3.left;
                break;
            case ESwipeDirection.UpRight:
                Debug.Log("Down");
                direction = Vector3.up + Vector3.right;
                break;
            case ESwipeDirection.DownLeft:
                Debug.Log("Left");
                direction = Vector3.down + Vector3.left;
                break;
            case ESwipeDirection.DownRight:
                Debug.Log("Right");
                direction = Vector3.down + Vector3.right;
                break;
        }
        this._targetPosition += direction * 5;
    }

    private void Update() {
        if(this.transform.position != this._targetPosition) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetPosition, this._speed * Time.deltaTime);
            
        }
    }

}
