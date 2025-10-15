using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services
{
    public interface ISessionService 
    {
        IEnumerable<SessionViewModel> GetAllSessions();
       
    }
}
