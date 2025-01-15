using API.Data;
using API.DTO;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("GetApivilla")]
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository<Villa> _repository;
        public VillaController(ApplicationDbContext dbContext,Repository<Villa> repository ) { 
        
            _context = dbContext;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<Villa> GetVilla()
        {
            var Villa = _repository.GetEverythingElment();      
            
            return Ok(Villa);
        }

        [HttpGet("{idvilla}")]
        public ActionResult<List<Villa>> GetVillaByid(int idvilla)
        {
            var obj = _context.Villas.FirstOrDefault(e => e.Id == idvilla);

            if (obj == null)
            {
                return NotFound("data not found");
            }
            return Ok(obj);
        }

        [HttpPost]
        public ActionResult<VillaDTO> PostVilla([FromBody] VillaDTO villaDTO)
        {

            if (_context.Villas.FirstOrDefault(e => e.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Validation", "El nombre ya existe en la lista");
                return BadRequest(ModelState);
            }
            // Validar el modelo recibido
            if (villaDTO == null)
            {
                return BadRequest("El objeto Villa no puede ser nulo.");
            }
            // Validar el modelo usando atributos como [Required] si los tienes definidos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Crear una nueva instancia del objeto Villa
            var nuevaVilla = new Villa
            {               
                Name = villaDTO.Name,
                Detalle = villaDTO.Detalle,
                Tarifa = villaDTO.Tarifa,
                Ocupantes = villaDTO.Ocupantes,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                ImagenUrl = villaDTO.ImagenUrl,
                Amenidad = villaDTO.Amenidad,
            };
            // Agregar la nueva villa a la lista de datos
             _repository.Create(nuevaVilla); 
            // Retornar una respuesta con el recurso creado
            return CreatedAtAction(nameof(PostVilla), new { id = nuevaVilla.Id }, nuevaVilla);
        }


        [HttpDelete("{id}")]
        public ActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var vi = _context.Villas.FirstOrDefault(e => e.Id == id);

            if (id == null)
            {
                return BadRequest();
            }

            _repository.RemoveData(vi);

            return NoContent();
        }

        [HttpPatch("{villaid}")]

        public ActionResult<Villa> updatedataVilla(int villaid,[FromBody] VillaDTO villaDTO) { 
        
           if (villaDTO == null)
            {
                BadRequest();
            }

            var Villaexistente = _context.Villas.FirstOrDefault(e => e.Id == villaid );

            if (Villaexistente == null)
            {
                return NotFound($"No se encontró ninguna villa con el ID {villaid}.");
            }

            Villaexistente.Name = villaDTO.Name;
            Villaexistente.Ocupantes = villaDTO.Ocupantes;
            Villaexistente.MetrosCuadrados = villaDTO.Ocupantes;
            Villaexistente.Tarifa = villaDTO.Tarifa;
            Villaexistente.Detalle = villaDTO.Detalle;

            _context.SaveChanges();


            return Villaexistente;
        }
    }
}

