using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VDS.RDF.Query;
using VDS.RDF.Query.Paths;

namespace Hospital.Controllers
{
    public class HospitalController : Controller
    {
        private static SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://localhost:3030/Hospital/"));

        [HttpPost]
        [HttpGet]
        public IActionResult Index(string Id,string Enfermedad)
        {
            List<Paciente> Listal = new List<Paciente>();
            SparqlResultSet resultado = endpoint.QueryWithResultSet(
                "PREFIX dato: <http://www.semanticweb.org/diego/ontologies/2023/10/untitled-ontology-11#> " +
                "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" +
                "SELECT ?nombre_paciente ?Genero ?altura ?peso ?telefono ?id ?nomEnfer " +
                "WHERE {  " +
                    "?Hostp rdf:type dato:Paciente. " +
                    "?Hostp dato:Nombre ?nombre_paciente. " +
                    "?Hostp dato:Genero ?Genero. " +
                    "?Hostp dato:Altura ?altura. " +
                    "?Hostp dato:Peso ?peso. " +
                    "?Hostp dato:Telefono ?telefono. " +
                    "?Hostp dato:Indentificacion ?id. "+
                    "?Hostp dato:Padece ?enfermedad. " +
                    "?enfermedad dato:NombreEnfermedad ?nomEnfer. " +
                    (string.IsNullOrEmpty(Id) ? "" : $"FILTER(?id ={Id})") +
                    (string.IsNullOrEmpty(Enfermedad) ? "" : $"FILTER(contains(lcase(?nomEnfer), lcase('{Enfermedad}')))") +

            " }"
            );

            foreach (var result in resultado.Results)
            {
                Paciente paciente = new Paciente();
                var dato = result.ToList();
                paciente.Nombre = dato[0].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#string", "") ;
                paciente.Genero = dato[1].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#string", "");
                paciente.Altura = dato[2].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#decimal", "") ;
                paciente.Peso = dato[3].Value.ToString().Replace("e0^^http://www.w3.org/2001/XMLSchema#double", ""); 
                paciente.Telefono = dato[4].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#decimal", ""); 
                paciente.Identificacion = dato[5].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#int", "");
                paciente.Enfermedad = new Enfermedad() { 
                    Nombre = dato[6].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#string", "")
                }; 
                Listal.Add(paciente); 

            }



            return View(Listal);
            
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Enfermedades(string ser)
        {
            List<Enfermedad> Listal = new List<Enfermedad>();
            SparqlResultSet resultado = endpoint.QueryWithResultSet(
                "PREFIX dato: <http://www.semanticweb.org/diego/ontologies/2023/10/untitled-ontology-11#> " +
                "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>" +
                "SELECT  ?enfermedad (group_concat(distinct?nomequi; separator=',') as ?Detectados_Por) (group_concat(?nomser; separator=',') as ?Servicios) " +
                "WHERE {  " +
                    "?ser rdf:type dato:Enfermedad. " +
                    "?ser dato:NombreEnfermedad ?enfermedad. " +
                    "?ser dato:EsDiagnosticada ?equipo. " +
                    "?equipo dato:Nombre ?nomequi. " +
                    "?equipo dato:SonUsados ?Servicio. " +
                    "?Servicio dato:Nombre ?nomser. " +
                    //(string.IsNullOrEmpty(Id) ? "" : $"FILTER(?id ={Id})") +
                    (string.IsNullOrEmpty(ser) ? "" : $"FILTER(contains(lcase(?nomser), lcase('{ser}')))") +

            " }"+
            "group by ?enfermedad"
            );

            foreach (var result in resultado.Results)
            {
                Enfermedad enfer = new Enfermedad();
                var dato = result.ToList();
                enfer.Nombre = dato[0].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#string", "");
                enfer.equipos = new List<Equipos>();
                var aux= dato[1].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#string", "").Split(',');
                var aux1 = dato[2].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#string", "").Split(',');
                int i = 0;
                foreach (var item in aux)
                {
                    enfer.equipos.Add(new Equipos() { 
                        Nombre = item ,
                        servicios = new List<Servicios>()
                    });
                    foreach(var item1 in aux1)
                    {
                        enfer.equipos[i].servicios.Add(new Servicios()
                        {
                            Nombre = item1
                        });
                    }
                    i++;
                   
                }

                Listal.Add(enfer);
            }
            return View(Listal);
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Servicio(string equipo)
        {
            List<Servicios> Listal = new List<Servicios>();
            SparqlResultSet resultado = endpoint.QueryWithResultSet(
                "PREFIX dato: <http://www.semanticweb.org/diego/ontologies/2023/10/untitled-ontology-11#> " +
                "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                "SELECT ?nomser (group_concat(distinct?nomequi; separator=',') as ?a) " +
                "WHERE {  " +
                    "?ser rdf:type dato:Servicios. " +
                    "?ser dato:Nombre ?nomser. " +
                    "?ser dato:Usan ?equi. " +
                    "?equi dato:Nombre ?nomequi. " +
                    //(string.IsNullOrEmpty(Id) ? "" : $"FILTER(?id ={Id})") +
                    (string.IsNullOrEmpty(equipo) ? "" : $"FILTER(contains(lcase(?nomequi), lcase('{equipo}')))") +
                " }" +
                "group by ?nomser "+
                "order by ?nomser "
            );

            foreach (var result in resultado.Results)
            {
                Servicios ser = new Servicios();
                var dato = result.ToList();
                ser.Nombre = dato[0].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#string", "");
                ser.equipos = new List<Equipos>();
                var aux = dato[1].Value.ToString().Replace("^^http://www.w3.org/2001/XMLSchema#string", "").Split(',');
                int i = 0;
                foreach (var item in aux)
                {
                    ser.equipos.Add(new Equipos()
                    {
                        Nombre = item,
                    });
  // hola 

                }

                Listal.Add(ser);
            }
            return View(Listal);
        }

        public IActionResult Clases()
        {
            List<string> Lista1 = new List<string>();
            List<string> Lista2 = new List<string>();
            List<string> Lista3 = new List<string>();

            SparqlResultSet resultado = endpoint.QueryWithResultSet(
                "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>" +
                "PREFIX owl: <http://www.w3.org/2002/07/owl#> " +
                "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> " +
                "SELECT DISTINCT ?class ?label ?description " +
                "WHERE {  " +
                    "?class a owl:Class. " +
                    "OPTIONAL { ?class rdfs:label ?label} " +
                    "OPTIONAL { ?class rdfs:comment ?description} " +
                " }" 
            );
            foreach (var result in resultado.Results)
            {
                var dato = result.ToList();
                Lista1.Add(WebUtility.UrlDecode(dato[0].Value.ToString().Replace("http://www.semanticweb.org/diego/ontologies/2023/10/untitled-ontology-11#", "")));

            }
            ViewBag.Lista1 = Lista1;
            ViewBag.Lista2 = Lista2;
            ViewBag.Lista3 = Lista3;
            return View();
        }

    }
}
