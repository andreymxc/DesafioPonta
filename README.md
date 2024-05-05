Para executar a API localmente em sua máquina, siga estes passos:

Abra o Visual Studio 2022 e clique em "Clone a Repository";

Na seção "repository location", cole o link do repositório: https://github.com/andreymxc/DesafioPonta;

Escolha uma pasta VAZIA em seu diretório preferido em "Path" e clique em "Clone";

Depois que a solução for aberta, selecione o projeto DesafioPonta.Api (Projeto ASP.NET Core) como o projeto inicial;

Para garantir que a API funcione perfeitamente em sua máquina, não se esqueça de restaurar os pacotes do NuGet após clonar o repositório. Você pode fazer isso facilmente executando o comando dotnet restore no terminal do Visual Studio ou através do gerenciador de pacotes NuGet. Este passo é essencial para garantir que todas as dependências estejam corretamente instaladas e a aplicação possa ser executada sem problemas.

Execute a aplicação via ISS Express;

Ao ser direcionado para o Swagger, registre-se como usuário no endpoint /Usuario/Cadastrar. Precisaremos de um e-mail e uma senha forte.

Depois de se cadastrar, use as mesmas informações inseridas para fazer login no endpoint /Usuario/Token. Esse endpoint retornará o token na propriedade "access_token".

Após obter o token, clique no cadeado no topo da tela e no valor do textbox informe "Bearer " seguido pelo token. Exemplo: Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.

Com esses passos concluídos, você estará autenticado na aplicação e pronto para realizar o CRUD de tarefas.
