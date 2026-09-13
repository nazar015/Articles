using Articles.Application.Repositories;
using MediatR;

namespace Articles.Application.Commands.DeleteArticle;

public class DeleteArticleHandler(IArticleRepository repository): IRequestHandler<DeleteArticleCommand, bool>
{
    public async Task<bool> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var deleted = await repository.DeleteAsync(request.Id);
        return deleted;
    }
}