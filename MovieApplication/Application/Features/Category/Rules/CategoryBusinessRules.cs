using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Category.Constants;
using Application.Services.Repositories;
using CoreApplication.Rules;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Domain.Entities;

namespace Application.Features.Category.Rules
{
    public class CategoryBusinessRules : BaseBusinessRules
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryBusinessRules(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task CategoryNameCannotBeDublicatedWhenInserted(string name)
        {
            Domain.Entities.Category category = await _categoryRepository.GetAsync(predicate: n => n.CategoryName == name);

            if (category != null) throw new BusinessException(CategoryMessages.CategoryNameExists);
        }

        public async Task NoCategoryFound(Guid Id)
        {
            Domain.Entities.Category category = await _categoryRepository.GetAsync(predicate: n => n.EntityID == Id);

            if (category == null) throw new BusinessException(CategoryMessages.NoCategoryFound);
        }
    }
}
