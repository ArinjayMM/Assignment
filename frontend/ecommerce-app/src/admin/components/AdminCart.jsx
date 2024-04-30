import axios from 'axios'
import React, { useEffect, useState } from 'react'
import AdminNavBar from '../pages/AdminNavBar'

function AdminCart() {
  const [cartItems, setCartItems] = useState([])

  useEffect(() => {
    loadCartItems()
  }, [])

  const loadCartItems = async () => {
    try {
      const response = await axios.get('https://localhost:7253/api/Cart')
      setCartItems(response.data)
    } catch (error) {
      console.error('Error loading cart items:', error)
    }
  }

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('en-IN', {
      maximumFractionDigits: 2,
      minimumFractionDigits: 2,
    }).format(value)
  }

  const deleteCartItem = async (id) => {
    try {
      await axios.delete(`https://localhost:7253/api/Cart/${id}`)
      setCartItems(cartItems.filter((item) => item.id !== id))
      console.log('Item deleted successfully')
    } catch (error) {
      console.error('Error deleting cart item:', error)
    }
  }

  return (
    <div>
      <AdminNavBar />
      <h2 className='text-center mt-3'>Cart Items</h2>
      <div className='container'>
        <table className='table table-striped'>
          <thead>
            <tr>
              <th scope='col'>Cart Item ID</th>
              <th scope='col'>User ID</th>
              <th scope='col'>Product ID</th>
              <th scope='col'>Unit Price ()</th>
              <th scope='col'>Discount (%)</th>
              <th scope='col'>Quantity</th>
              <th scope='col'>Total Price</th>
            </tr>
          </thead>
          <tbody>
            {cartItems.map((item) => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.userId}</td>
                <td>{item.productID}</td>
                <td>{formatCurrency(item.unitPrice)}</td>
                <td>{item.discount}</td>
                <td>{item.quantity}</td>
                <td>{formatCurrency(item.totalPrice)}</td>

                <td>
                  <button
                    className='btn btn-danger btn-sm'
                    onClick={() => deleteCartItem(item.id)}
                  >
                    Remove
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

export default AdminCart
