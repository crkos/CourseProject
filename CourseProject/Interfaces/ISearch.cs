namespace CourseProject.Interfaces
{
    public interface ISearch<T>
    {
        IEnumerable<T> Search(string query);
    }
}
