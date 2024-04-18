using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class CustomerService
{
    private readonly HotelDbContext _context;

    public CustomerService(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync() {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int customerId) {
        return await _context.Customers.FindAsync(customerId);
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer) {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer) {
        _context.Entry(customer).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task DeleteCustomerAsync(int customerId) {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer != null) {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}