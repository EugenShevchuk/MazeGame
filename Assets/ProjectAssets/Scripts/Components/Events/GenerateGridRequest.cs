using Project.Infrastructure;

namespace Project.Events
{
    public struct GenerateGridRequest
    {
        public Level Level;
        public int Rows;
        public int Columns;
    }
}