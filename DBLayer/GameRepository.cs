using System;
using System.Collections.Generic;
using System.Linq;

namespace DBLayer
{
    public interface IGameRepository
    {
        HanoiTower GetGame(int Id);
        bool InsertGame(HanoiTower gameItem);
        bool UpdateGame(HanoiTower gameItem);
        bool DeleteGame(int id);
    }
    public class GameRepository : IGameRepository
    {
        private GameEntities DbContext { get; set; }
        public GameRepository(GameEntities dbContext)
        {
            this.DbContext = dbContext;
        }

        public HanoiTower GetGame(int Id)
        {
            return DbContext.HanoiTowers.Where(p => p.Id == Id).FirstOrDefault();
        }
        public bool InsertGame(HanoiTower gameItem)
        {
            bool status;
            try
            {
                DbContext.HanoiTowers.Add(gameItem);
                DbContext.SaveChanges();
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public bool UpdateGame(HanoiTower gameItem)
        {
            bool status;
            try
            {
                HanoiTower prodItem = DbContext.HanoiTowers.Where(p => p.Id == gameItem.Id).FirstOrDefault();
                if (prodItem != null)
                {
                    prodItem.Definition = gameItem.Definition;
                    prodItem.Message = gameItem.Message;
                    prodItem.Status = gameItem.Status;
                    DbContext.SaveChanges();
                }
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public bool DeleteGame(int id)
        {
            bool status;
            try
            {
                HanoiTower gameItem = DbContext.HanoiTowers.Where(p => p.Id == id).FirstOrDefault();
                if (gameItem != null)
                {
                    DbContext.HanoiTowers.Remove(gameItem);
                    DbContext.SaveChanges();
                }
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
    }
}
