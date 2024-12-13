﻿using FamilyTreeBlazor.BLL.DTOs;
using FamilyTreeBlazor.DAL.Infrastructure;
using FamilyTreeBlazor.DAL.Entities;

namespace FamilyTreeBlazor.BLL;

public class PersonService(IRepository<Person> personRepository, TreeCacheDTO treeCache)
{
    private readonly IRepository<Person> _personRepository = personRepository;
    private readonly TreeCacheDTO _treeCache = treeCache;

    public async Task AddPersonAsync(PersonDTO person)
    {
        var entity = new Person(person.Id, person.Name, person.BirthDateTime, person.Sex);
        
        await _personRepository.AddAsync(entity);
        _treeCache.Persons[person.Id] = person;
    }

    public PersonDTO? GetPersonById(int id)
    {
        return _treeCache.Persons.TryGetValue(id, out var person) ? person : null;
    }

    public async Task<IEnumerable<PersonDTO>> GetAllPersonsAsync() 
    { 
        var persons = await _personRepository.GetAllAsync();

        return persons.Select(person => new PersonDTO(person.Id, person.Name, person.BirthDateTime, person.Sex));
    }

    public async Task UpdatePersonAsync(PersonDTO person)
    {
        var entity = new Person(person.Id, person.Name, person.BirthDateTime, person.Sex);
        
        await _personRepository.UpdateAsync(entity);
        _treeCache.Persons[person.Id] = person;
    }

    public async Task DeletePersonAsync(int id)
    {
        await _personRepository.DeleteAsync(id);
        _treeCache.Persons.Remove(id);
    }

    public async Task ClearAllAsync()
    {
        await _personRepository.TruncateTableAsync();
    }
        
}
