using GolfCentra.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GolfCentra.Core.GenericRepository
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> where TEntity : class
    {
        #region Private member variables...
        internal GolfCentraEntities context;
        internal IDbSet<TEntity> entities;
        #endregion

        #region Public Constructor...
        /// <summary>
        /// Public Constructor,initializes privately declared local variables.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(GolfCentraEntities context)
        {
            this.context = context;
        }
        #endregion

        #region Public member methods...

        /// <summary>
        /// Generic get method on the basis of id for Entities.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(object id)
        {
            return this.Entities.Find(id);
        }

        /// <summary>
        /// generic Get method for Entities
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = Entities;
            return query.ToList();
        }

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TEntity Get(Func<TEntity, Boolean> where)
        {
            return this.Entities.Where(where).FirstOrDefault<TEntity>();
        }
        /// <summary>
        /// Get  
        /// </summary>
        /// <param name="wherePredicate"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public TEntity GetWithInclude(Expression<Func<TEntity, bool>> wherePredicate, params string[] includeProperties)
        {
            IQueryable<TEntity> query = Entities;
            query = includeProperties.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(wherePredicate).FirstOrDefault();
        }

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetManyWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = Entities;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);

        }

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetManyWithInclude(params string[] include)
        {
            IQueryable<TEntity> query = Entities;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query;
        }


        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return Entities.Where(where).ToList();
        }

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return this.Entities.ToList();
        }

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(TEntity entity)
        {
            Entities.Add(entity);
        }

        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public void Update(TEntity entity)
        {
            Entities.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                Entities.Attach(entity);
            }
            Entities.Remove(entity);
        }

        /// <summary>
        /// Generic Delete method for the entities
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(long id)
        {
            TEntity entityToDelete = Entities.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// generic method to fetch Last the records from db
        /// </summary>
        /// <returns></returns>
        public virtual TEntity GetLastRecord()
        {
            return this.Entities.ToList().LastOrDefault();
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return Entities.Where(where).AsQueryable();
        }
        #endregion

        private IDbSet<TEntity> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = context.Set<TEntity>();
                }
                return entities;
            }
        }
    }
}



