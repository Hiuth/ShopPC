using ShopPC.Data;
using Microsoft.EntityFrameworkCore;
using ShopPC.Repository.InterfaceRepository;
using System.Linq.Expressions;

namespace ShopPC.Repository.ImplementationsRepository
{
    public class GenericReopository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context; // đối tượng kết nối database
        protected readonly DbSet<T> _dbSet; // đại diện cho bảng tương ứng với enity T trong database

        public GenericReopository(AppDbContext context) // hàm khởi tạo
        {
            _context = context;
            _dbSet = _context.Set<T>(); // gán DbSet tương ứng với entity T
            //VD: nếu T = Product → _dbSet = context.Products
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            //includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries):
            //Phương thức Split này chia chuỗi includeProperties thành một mảng các chuỗi con, sử dụng dấu phẩy (',') làm ký tự phân tách.
            //StringSplitOptions.RemoveEmptyEntries loại bỏ các phần tử rỗng (ví dụ: nếu chuỗi là "Role,,Permissions", chỉ giữ "Role" và "Permissions").

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<T> AddAsync(T entity)// hàm thêm dữ liệu
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity); // gắn entity vào context để theo dõi
            _context.Entry(entity).State = EntityState.Modified; // đánh dấu entity là đã sửa đổi
            await _context.SaveChangesAsync(); // lưu thay đổi vào database
        }

        public virtual async Task DeleteAsync(string id)
        {
            T? entity = await GetByIdAsync(id); // tìm entity theo id
            if (entity != null)
            {
                _dbSet.Remove(entity); // xóa entity khỏi DbSet
                await _context.SaveChangesAsync(); // lưu thay đổi vào database
            }
        }

        public virtual async Task<bool> ExistsAsync(string id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter != null)
            {
                return await _dbSet.CountAsync(filter);
            }
            else
            {
                return await _dbSet.CountAsync();
            }
        }
    }
}
