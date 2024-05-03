Instrução de como executar a API localmente em sua máquina:

Abra o Visual Studio 2022 e selecione a opção "Clone a Repository";

Em repository location, preencher com o link do repositório: https://github.com/andreymxc/DesafioPonta.v2.git
No Path, selecionar uma pasta VAZIA em um diretório de sua preferência e depois clique em "Clone";

Após a solução ser aberta, selecione o projeto DesafioPonta.Api (Projeto ASP.NET Core) como o o Start Up Project;

Execute a aplicação via ISS Express;

Ao ser direcionado para o Swagger, registre seu usuário no endpoint /Usuario/Cadastrar. É necessário um e-mail e uma senha forte.

Após realizado o cadastro, use os mesmos dados inseridos no cadastro para se autenticar pelo endpoint /Usuario/Token. 
Este endpoint retornará o token na propriedade "access_token".

Após obter o token, clique no cadeado no topo da tela e no valor do textbox informe "Bearer <seuToken>"
Exemplo: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.

Após concluído este passos, você estará autenticado na aplicação e poderá realizar as operações envolvendo as tarefas!


