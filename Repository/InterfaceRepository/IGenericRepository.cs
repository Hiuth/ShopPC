using ShopPC.Models;
using System.Linq.Expressions;

namespace ShopPC.Repository.InterfaceRepository
{
    public interface IGenericRepository<T> where T: class
    {
        Task<T?> GetByIdAsync(string id);

        Task<IEnumerable<T>> GetAllAsync();

        //Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(string id);

        Task<bool> ExistsAsync(string id);

        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

        Task<IEnumerable<T>> GetAsync(  // hàm get tất cả các thể loại (chấp hết)
            Expression<Func<T, bool>>? filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "");


        //Trả về một danh sách (IEnumerable) các đối tượng kiểu T. T là generic type(ví dụ: Product, Category, User…).

        //Vì có Task<>, hàm này bất đồng bộ(async) → giúp API không bị nghẽn khi truy vấn DB. Nghĩa là: Hàm này sẽ trả về một danh sách các bản ghi(kiểu T) từ database, sau khi truy vấn xong.

        //Expression<Func<T, bool>>? filter = null: Đây là biểu thức điều kiện (filter) tương tự như WHERE trong SQL.
        //Func<T, bool> nghĩa là một hàm nhận T và trả về true/false. Expression<> giúp Entity Framework dịch biểu thức này sang SQL.

        //Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null: Đây là hàm để sắp xếp kết quả (tương tự như ORDER BY trong SQL).
        //IQueryable<T> cho phép xây dựng truy vấn LINQ (ví dụ .OrderBy(...) hoặc .OrderByDescending(...)). IOrderedQueryable<T> là kết quả sau khi đã sắp xếp.


        //string includeProperties = "": Dùng để load thêm các navigation properties (quan hệ) — giống JOIN trong SQL. Cách hoạt động: bạn truyền tên property dạng chuỗi, EF sẽ .Include() nó
    }
}
