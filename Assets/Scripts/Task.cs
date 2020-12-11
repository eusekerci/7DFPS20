using UnityEngine;

public enum TaskStatus
{
    Todo,
    Progress,
    Done
}

public class Task : MonoBehaviour
{
    private float _progressionSpeed = 1;
    private float _taskSize = 10;
    private float _currentProgress = 0;

    private TaskStatus _currentStatus = TaskStatus.Todo;

    void FixedUpdate()
    {
        if (_currentStatus == TaskStatus.Progress)
        {
            _currentProgress += _progressionSpeed * Time.deltaTime;
            if (_currentProgress >= _taskSize)
            {
                _currentProgress = _taskSize;
                _currentStatus = TaskStatus.Done;
            }
        }
    }

    public void StartTask()
    {
        _currentStatus = TaskStatus.Progress;
    }
}
