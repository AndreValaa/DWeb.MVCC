    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using DWeb_MVC.Data;
    using DWeb_MVC.Models;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;

    namespace DWeb_MVC.Controllers
    {

        public class ProdutosController : Controller
        {
            private readonly ApplicationDbContext _bd;

            /// <summary>
            /// este recurso vai proporcionar acesso aos dados dos 
            /// recursos do servidor
            /// </summary>
            private readonly IWebHostEnvironment _webHostEnvironment;

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var email = context.HttpContext.User.Identity?.Name?.ToLower();
                if (email != "jose1@gmail.com")
                {
                    context.Result = new RedirectToActionResult("UserHome", "Home", null);
                }

                base.OnActionExecuting(context);
            }


            public ProdutosController(
                ApplicationDbContext context,
                IWebHostEnvironment webHostEnvironment
                )
            {
                _bd = context;
                _webHostEnvironment = webHostEnvironment;
            }

            // GET: Produtos
            public async Task<IActionResult> Index()
            {
            /* procurar, na base de dados, a lista dos produtos existentes
          * SELECT *
          * FROM Produtos a INNER JOIN Categoria c ON a.Categoria = c.Id
          */
            var listaProdutos = _bd.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Cores); 

            return View(await listaProdutos.ToListAsync());
            }

            // GET: Produtos/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null || _bd.Produtos == null)
                {
                    return NotFound();
                }

                var produtos = await _bd.Produtos
                    .Include(p => p.Categoria)
                    .Include(p => p.Fotos)
                    .Include(p => p.Cores)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (produtos == null)
                {
                    return NotFound();
                }

                return View(produtos);
            }

            public async Task<IActionResult> Search(string id)
            {
                if (id == null || _bd.Produtos == null)
                {
                    return NotFound();
                }

                var produtos = await _bd.Produtos
                    .Include(p => p.Categoria)
                    .Include(p => p.Fotos)
                    .Include(p => p.Cores)
                    .FirstOrDefaultAsync(m => m.Nome.Equals(id));
                if (produtos == null)
                {
                    return NotFound();
                }

                return View(produtos);
            }

            // GET: Produtos/Create
            public IActionResult Create()
            {
                // obter a lista de professores existentes na BD
                ViewData["ListaCat"] = _bd.Categorias.OrderBy(c => c.Nome).ToList();
                ViewData["ListaCores"] = _bd.Cores.OrderBy(c => c.Nome).ToList();

            return View();
            }

            // POST: Produtos/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("Id,Nome,Marca,Preco,PrecoAux")] Produtos produtos, int[] listaIdsCategorias, int[] listaIdsCores, IFormFile imagemProduto)
            {

                // Encontrar as categorias selecionadas e adicioná-las ao produto
                var listaCategorias = new List<Categorias>();
                foreach (var catId in listaIdsCategorias)
                {
                    var cat = _bd.Categorias.FirstOrDefault(p => p.Id == catId);

                    if (cat != null)
                    {
                        listaCategorias.Add(cat);
                    }
                }

                // Encontrar as cores selecionadas e adicioná-las ao produto

                var listaCores = new List<Cores>();
                foreach (var corId in listaIdsCores)
                {
                    var cor = _bd.Cores.FirstOrDefault(c => c.Id == corId);
                    if (cor != null)
                    {
                        listaCores.Add(cor);
                    }
                }
                produtos.Cores = listaCores;

            // vars. auxiliares
            string nomeFoto = "";
                bool existeFoto = false;

            // avaliar se temos condições para tentar adicionar o produto
            // testar se a Categoria do produto != 0 
            if (listaCategorias == null || listaCategorias.Count == 0)
            {
                ModelState.AddModelError("", "É obrigatório escolher uma Categoria.");
            }

            if (listaCores == null || listaCores.Count == 0)
            {
                ModelState.AddModelError("", "É obrigatório escolher pelo menos uma Cor.");
            }

            // Apenas se ambas forem válidas, atribui
            if (ModelState.IsValid)
            {
                produtos.Categoria = listaCategorias;
                produtos.Cores = listaCores;
                // se cheguei aqui, escolhi Categoria
                // será q escolhi Imagem? Vamos avaliar...

                if (imagemProduto == null)
                    {
                        // o utilizador não fez upload de uma imagem
                        // vamos adicionar uma imagem prédefinida ao produto
                        produtos.Fotos
                                .Add(new Fotografias
                                {
                                    Data = DateTime.Now,
                                    Local = "NoImage",
                                    NomeFicheiro = "noProduto.jpg"
                                });
                    }
                    else
                    {
                        // há ficheiro. Mas, será que é uma imagem?
                        if (imagemProduto.ContentType != "image/jpeg" &&
                            imagemProduto.ContentType != "image/png")
                        {
                            //  <=>  ! (imagemProduto.ContentType == "image/jpeg" || imagemProduto.ContentType == "image/png")
                            // o ficheiro carregado não é uma imagem
                            // o que fazer?
                            // Vamos fazer o mesmo que quando o utilizador não
                            // fornece uma imagem
                            produtos.Fotos
                                    .Add(new Fotografias
                                    {
                                        Data = DateTime.Now,
                                        Local = "NoImage",
                                        NomeFicheiro = "noProduto.jpg"
                                    });
                        }
                        else
                        {
                            // há imagem!!!
                            // determinar o nome da imagem
                            Guid g = Guid.NewGuid();
                            nomeFoto = g.ToString();
                            // obter a extensão do ficheiro
                            string extensaoNomeFoto = Path.GetExtension(imagemProduto.FileName).ToLower();
                            nomeFoto += extensaoNomeFoto;

                            // guardar os dados do ficheiro na BD
                            // para isso, vou associá-los ao 'produto'
                            produtos.Fotos
                                    .Add(new Fotografias
                                    {
                                        Data = DateTime.Now,
                                        Local = "",
                                        NomeFicheiro = nomeFoto
                                    });

                            // informar a aplicação que há um ficheiro
                            // (imagem) para guardar no disco rígido
                            existeFoto = true;
                        }
                    } // if (imagemProduto == null)
                } // if (produto.Categoria == 0)

                //atribuir os dados do PrecoAux ao Preco
                //se nao for nulo
                if (!string.IsNullOrEmpty(produtos.PrecoAux))
                {
                    produtos.Preco = Convert.ToDecimal(produtos.PrecoAux.Replace('.', ','));
                }

                // se os dados recebidos respeitarem o modelo,
                // os dados podem ser adicionados
                if (ModelState.IsValid)
                {

                    try
                    {
                        // adicionar os dados à BD
                        _bd.Add(produtos);
                        // COMMIT da ação anterior
                        await _bd.SaveChangesAsync();

                        // se cheguei aqui, já foram guardados os dados
                        // do animal na BD. Já posso guardar a imagem
                        // no disco rígido do servidor
                        if (existeFoto)
                        {
                            // determinar onde guardar a imagem
                            string nomeLocalizacaoImagem = _webHostEnvironment.WebRootPath;
                            nomeLocalizacaoImagem = Path.Combine(nomeLocalizacaoImagem, "imagens");

                            // e, a pasta onde se pretende guardar a imagem existe?
                            if (!Directory.Exists(nomeLocalizacaoImagem))
                            {
                                Directory.CreateDirectory(nomeLocalizacaoImagem);
                            }

                            // informar o servidor do nome do ficheiro
                            string nomeDoFicheiro =
                               Path.Combine(nomeLocalizacaoImagem, nomeFoto);

                            // guardar o ficheiro
                            using var stream = new FileStream(nomeDoFicheiro, FileMode.Create);
                            await imagemProduto.CopyToAsync(stream);
                        }
                        // devolver o controlo da app para a página de início
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("",
                           "Ocorreu um erro com a adição dos dados do(a) " + produtos.Nome);
                        // throw;
                    }

                }
                ViewData["ListaCat"] = _bd.Categorias.OrderBy(c => c.Nome).ToList();
                ViewData["ListaCores"] = _bd.Cores.OrderBy(c => c.Nome).ToList();
            return View(produtos);
            }

            // GET: Produtos/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null || _bd.Produtos == null)
                {
                    return NotFound();
                }

                // Inclua as fotos ao carregar o produto
                var produtos = await _bd.Produtos
                                        .Include(p => p.Fotos) // Inclui as fotos
                                        .FirstOrDefaultAsync(m => m.Id == id);

                if (produtos == null)
                {
                    return NotFound();
                }

                produtos.PrecoAux = Convert.ToString(produtos.Preco);

                ViewData["ListaCat"] = _bd.Categorias.OrderBy(c => c.Nome).ToList();
                ViewData["ListaCores"] = _bd.Cores.OrderBy(c => c.Nome).ToList();

            return View(produtos);
            }


            // POST: Produtos/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Marca,Preco,PrecoAux")] Produtos produtos, int[] listaIdsCategorias, int[] listaIdsCores, IFormFile imagemProduto2)
            {
                if (id != produtos.Id)
                {
                    return NotFound();
                }

                // Encontrar o produto existente
                var produtoExistente = await _bd.Produtos
                    .Include(p => p.Categoria)
                    .Include(p => p.Fotos)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (produtoExistente == null)
                {
                    return NotFound();
                }

                // Atualizar as propriedades do produto
                produtoExistente.Nome = produtos.Nome;
                produtoExistente.Marca = produtos.Marca;
                produtoExistente.Preco = !string.IsNullOrEmpty(produtos.PrecoAux)
                    ? Convert.ToDecimal(produtos.PrecoAux)
                    : produtoExistente.Preco;

                // Atualizar categorias
                if (listaIdsCategorias != null && listaIdsCategorias.Length > 0)
                {
                    // Obter categorias selecionadas
                    var categoriasSelecionadas = await _bd.Categorias
                        .Where(c => listaIdsCategorias.Contains(c.Id))
                        .ToListAsync();

                    // Verificar e adicionar categorias novas
                    foreach (var categoria in categoriasSelecionadas)
                    {
                        if (!produtoExistente.Categoria.Contains(categoria))
                        {
                            produtoExistente.Categoria.Add(categoria);
                        }
                    }

                    // Remover categorias que não estão mais selecionadas
                    var categoriasParaRemover = produtoExistente.Categoria
                        .Where(c => !listaIdsCategorias.Contains(c.Id))
                        .ToList();

                    foreach (var categoria in categoriasParaRemover)
                    {
                        produtoExistente.Categoria.Remove(categoria);
                    }
                }

            // Atualizar cores
            if (listaIdsCores != null && listaIdsCores.Length > 0)
            {
                var coresSelecionadas = await _bd.Cores
                    .Where(c => listaIdsCores.Contains(c.Id))
                    .ToListAsync();

                // Adicionar cores novas
                foreach (var cor in coresSelecionadas)
                {
                    if (!produtoExistente.Cores.Contains(cor))
                    {
                        produtoExistente.Cores.Add(cor);
                    }
                }

                // Remover cores que já não estão selecionadas
                var coresParaRemover = produtoExistente.Cores
                    .Where(c => !listaIdsCores.Contains(c.Id))
                    .ToList();

                foreach (var cor in coresParaRemover)
                {
                    produtoExistente.Cores.Remove(cor);
                }
            }


            // Caso nenhuma categoria seja selecionada, mantenha as categorias existentes

            //---------- Para Foto ------------

            // vars. auxiliares
            string nomeFoto = "";
                bool existeFoto = false;

                // avaliar se temos condições para tentar adicionar a foto
                if (imagemProduto2 != null)
                {
                    // Verificar se é uma imagem
                    if (imagemProduto2.ContentType == "image/jpeg" || imagemProduto2.ContentType == "image/png")
                    {
                        // determinar o nome da imagem
                        Guid g = Guid.NewGuid();
                        nomeFoto = g.ToString();
                        string extensaoNomeFoto = Path.GetExtension(imagemProduto2.FileName).ToLower();
                        nomeFoto += extensaoNomeFoto;

                        // Adicionar foto ao produto
                        produtoExistente.Fotos.Add(new Fotografias
                        {
                            Data = DateTime.Now,
                            Local = "",
                            NomeFicheiro = nomeFoto
                        });

                        existeFoto = true;
                    }
                    else
                    {
                        // Caso o arquivo não seja uma imagem válida, adicionar imagem padrão
                        produtoExistente.Fotos.Add(new Fotografias
                        {
                            Data = DateTime.Now,
                            Local = "NoImage",
                            NomeFicheiro = "noProduto.jpg"
                        });
                    }
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _bd.Update(produtoExistente);
                        await _bd.SaveChangesAsync();

                        if (existeFoto)
                        {
                            // Determinar onde guardar a imagem
                            string nomeLocalizacaoImagem = _webHostEnvironment.WebRootPath;
                            nomeLocalizacaoImagem = Path.Combine(nomeLocalizacaoImagem, "imagens");

                            if (!Directory.Exists(nomeLocalizacaoImagem))
                            {
                                Directory.CreateDirectory(nomeLocalizacaoImagem);
                            }

                            // Informar o servidor do nome do ficheiro
                            string nomeDoFicheiro = Path.Combine(nomeLocalizacaoImagem, nomeFoto);

                            // Guardar o ficheiro
                            using var stream = new FileStream(nomeDoFicheiro, FileMode.Create);
                            await imagemProduto2.CopyToAsync(stream);
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProdutosExists(produtoExistente.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }

                ViewData["ListaCat"] = _bd.Categorias.OrderBy(c => c.Nome).ToList();
                ViewData["ListaCores"] = _bd.Cores.OrderBy(c => c.Nome).ToList();
            return View(produtoExistente);
            }


            // GET: Produtos/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null || _bd.Produtos == null)
                {
                    return NotFound();
                }

                var produtos = await _bd.Produtos
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (produtos == null)
                {
                    return NotFound();
                }

                return View(produtos);
            }

            // POST: Produtos/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                if (_bd.Produtos == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Produtos'  is null.");
                }
                var produtos = await _bd.Produtos.FindAsync(id);
                if (produtos != null)
                {
                    _bd.Produtos.Remove(produtos);
                }

                await _bd.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool ProdutosExists(int id)
            {
                return (_bd.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
            }


            //Deleta fotos
            [HttpDelete]
            public async Task<IActionResult> DeletePhoto(int id)
            {
                var foto = await _bd.Fotografias.FindAsync(id);
                if (foto == null)
                {
                    return NotFound();
                }

                // Remover a foto do sistema de arquivos, se necessário
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "imagens", foto.NomeFicheiro);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Remover a foto do banco de dados
                _bd.Fotografias.Remove(foto);
                await _bd.SaveChangesAsync();

                return Ok();
            }
        }
    }
