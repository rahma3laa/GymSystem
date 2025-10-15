using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services
{
    public interface ISessionService 
    {
        IEnumerable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionById(int sessionId);

        bool CreateSession(CreateSessionViewModel CreatedSession);
       
    }
}
