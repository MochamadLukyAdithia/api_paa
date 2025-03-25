using Microsoft.AspNetCore.Mvc;
using praktimupaa2.Helpers;
using praktimupaa2.Models.Auth;
using praktimupaa2.Models.Login;
using praktimupaa2.Models.Person;

namespace praktimupaa2.Controllers
{
    public class AuthController : Controller
    {
        private readonly string __constr;
        private readonly IConfiguration _config;
        public IActionResult Index()
        {
            return View();
        }

        public AuthController(IConfiguration config)
        {
            _config = config;
            __constr = _config.GetConnectionString("DefaultConnection");
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] Person registerData)
        {

            if (string.IsNullOrEmpty(registerData.email) || string.IsNullOrEmpty(registerData.password))
            {
                return BadRequest(new { message = "Email dan password harus diisi" });
            }

            AuthContext context = new AuthContext(__constr);


            Person existingPerson = context.Auth(registerData.email, registerData.password);
            if (existingPerson != null)
            {
                return BadRequest(new { message = "Email atau password sudah terdaftar" });
            }


            bool isRegistered = context.RegisterPerson(registerData);

            if (isRegistered)
            {
                return Ok(new { message = "Registrasi berhasil" });
            }
            else
            {
                return StatusCode(500, new { message = "Registrasi gagal" });
            }
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] Auth loginData)
        {
            AuthContext context = new AuthContext(__constr);
            Person person = context.Auth(loginData.email.ToString(), loginData.password.ToString());

            if (person == null )
            {
                return Unauthorized(new { message = "Email atau password salah"});
            }
          
            JwtHelper helper = new JwtHelper(_config);
            string token = helper.GenerateJwtToken(loginData);


            return Ok(new
            {
                token = token,
                person = new
                {
                    id = person.id_person,
                    email = person.email,
                    nama = person.nama
                }
            });
        }

    }
}
