using DbMasterD.Models.ViewModels;
using DbMasterD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace work_01_MasterDetails.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly IWebHostEnvironment _he;
        private readonly CandidateDbContext _context;
        public CandidatesController(IWebHostEnvironment _he, CandidateDbContext _context)
        {
            this._context = _context;
            this._he = _he;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _context.Candidates.Include(x => x.CandidateSkills).ThenInclude(y => y.Skill).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult AddNewSkills(int? id)
        {
            ViewBag.skill = new SelectList(_context.Skills, "SkillId", "SkillName", id.ToString() ?? "");
            return PartialView("_AddNewSkills");
        }
        [HttpPost]
        public async Task<IActionResult> Create(CandidateVM candidateVM, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Candidate candidate = new Candidate()
                {
                    CandidateName = candidateVM.CandidateName,
                    DateOfBirth = candidateVM.DateOfBirth,
                    Phone = candidateVM.Phone,
                    Fresher = candidateVM.Fresher
                };
                //image
                var file = candidateVM.ImagePath;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFileName = DateTime.Now.Ticks.ToString() + "_" + Path.GetFileName(candidateVM.ImagePath.FileName);
                string fileToSave = Path.Combine(webroot, folder, imgFileName);

                if (file != null)
                {
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        candidateVM.ImagePath.CopyTo(stream);
                        candidate.Image = "/" + folder + "/" + imgFileName;
                    }
                }
                foreach (var item in skillId)
                {
                    CandidateSkill candidateSkill = new CandidateSkill()
                    {
                        Candidate = candidate,
                        CandidateId = candidate.CandidateId,
                        SkillId = item
                    };
                    _context.CandidateSkills.Add(candidateSkill);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id);

            CandidateVM candidateVM = new CandidateVM()
            {
                CandidateId = candidate.CandidateId,
                CandidateName = candidate.CandidateName,
                DateOfBirth = candidate.DateOfBirth,
                Phone = candidate.Phone,
                Image = candidate.Image,
                Fresher = candidate.Fresher
            };
            var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == id).ToList();
            foreach (var item in existSkill)
            {
                candidateVM.SkillList.Add(item.SkillId);
            }
            return View(candidateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CandidateVM candidateVM, int[] SkillId)
        {
            if (ModelState.IsValid)
            {
                Candidate candidate = new Candidate()
                {
                    CandidateId = candidateVM.CandidateId,
                    CandidateName = candidateVM.CandidateName,
                    DateOfBirth = candidateVM.DateOfBirth,
                    Phone = candidateVM.Phone,
                    Fresher = candidateVM.Fresher,
                    Image = candidateVM.Image
                };
                var file = candidateVM.ImagePath;
                string existImg = candidateVM.Image;

                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = DateTime.Now.Ticks.ToString() + "_" + Path.GetFileName(candidateVM.ImagePath.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        candidateVM.ImagePath.CopyTo(stream);
                        candidate.Image = "/" + folder + "/" + imgFileName;
                    }

                }
                else
                {
                    candidate.Image = existImg;
                }

                var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).ToList();
                //Remove
                foreach (var item in existSkill)
                {
                    _context.CandidateSkills.Remove(item);
                }
                //Add
                foreach (var item in SkillId)
                {
                    CandidateSkill candidateSkill = new CandidateSkill()
                    {
                        CandidateId = candidate.CandidateId,
                        SkillId = item
                    };
                    _context.CandidateSkills.Add(candidateSkill);
                }
                _context.Update(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id);
            var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == id).ToList();
            foreach (var item in existSkill)
            {
                _context.CandidateSkills.Remove(item);
            }

            _context.Remove(candidate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
