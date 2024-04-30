import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import AdminNavBar from '../pages/AdminNavBar'

function AdminOrder({ orderId }) {
  const [orderItems, setOrderItems] = useState([])
  const [orderStatuses, setOrderStatuses] = useState({})
  const [editMode, setEditMode] = useState({})

  useEffect(() => {
    loadOrderItems()
  }, [])

  const loadOrderItems = async () => {
    try {
      const response = await axios.get('https://localhost:7253/api/Order')
      setOrderItems(response.data)
      // Initialize order statuses and edit mode for each item
      const initialStatuses = {}
      const initialEditMode = {}
      response.data.forEach((item) => {
        initialStatuses[item.id] = item.orderStatus
        initialEditMode[item.id] = false
      })
      setOrderStatuses(initialStatuses)
      setEditMode(initialEditMode)
    } catch (error) {
      console.error('Error loading order items:', error)
      toast.error('Error loading order items')
    }
  }

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('en-IN', {
      maximumFractionDigits: 2,
      minimumFractionDigits: 2,
    }).format(value)
  }

  const deleteOrderItem = async (id) => {
    try {
      await axios.delete(`https://localhost:7253/api/Order/${id}`)
      setOrderItems(orderItems.filter((item) => item.id !== id))
      const updatedStatuses = { ...orderStatuses }
      delete updatedStatuses[id]
      setOrderStatuses(updatedStatuses)
      console.log('Item deleted successfully')
    } catch (error) {
      console.error('Error deleting cart item:', error)
    }
  }

  const editOrderItem = async (id, selectedOrderStatus) => {
    try {
      console.log(
        'Editing order item:',
        id,
        'with status:',
        selectedOrderStatus
      )
      const response = await axios.put(
        `https://localhost:7253/api/Order/${id}`,
        null,
        {
          params: { orderStatus: selectedOrderStatus },
        }
      )

      console.log('Response:', response)

      if (response.status === 200) {
        console.log('Order status updated successfully')
        toggleEditMode(id)
        setOrderItems((prevOrderItems) =>
          prevOrderItems.map((item) =>
            item.id === id
              ? { ...item, orderStatus: selectedOrderStatus }
              : item
          )
        )
      } else {
        console.error('Failed to update order status:', response.data)
      }
    } catch (error) {
      console.error('Error updating order status:', error)
    }
  }

  const handleStatusChange = (id, status) => {
    const newStatus = status || 'Pending'
    console.log('New order status:', newStatus)
    setOrderStatuses((prevStatuses) => ({
      ...prevStatuses,
      [id]: newStatus,
    }))
  }

  const toggleEditMode = (id) => {
    setEditMode((prevEditMode) => ({
      ...prevEditMode,
      [id]: !prevEditMode[id],
    }))
  }

  return (
    <div>
      <AdminNavBar />
      <h2 className='text-center my-3'>Order Details</h2>
      <div className='container'>
        <table className='table table-striped'>
          <thead>
            <tr>
              <th scope='col'>Order Item ID</th>
              <th scope='col'>User ID</th>
              <th scope='col'>Order Number</th>
              <th scope='col'>Product ID</th>
              <th scope='col'>Quantity</th>
              <th scope='col'>Total Price</th>
              <th scope='col'>Order Status</th>
              <th scope='col'>Created On</th>
              <th scope='col'>Actions</th>
            </tr>
          </thead>
          <tbody>
            {orderItems.map((item) => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.userId}</td>
                <td>{item.orderNo}</td>
                <td>{item.productID}</td>
                <td>{item.quantity}</td>
                <td>{formatCurrency(item.totalPrice)}</td>
                <td>
                  {editMode[item.id] ? (
                    <select
                      value={orderStatuses[item.id] || ''}
                      onChange={(e) =>
                        handleStatusChange(item.id, e.target.value)
                      }
                    >
                      <option value='Pending'>Pending</option>
                      <option value='Processing'>Processing</option>
                      <option value='Shipped'>Shipped</option>
                      <option value='Delivered'>Delivered</option>
                    </select>
                  ) : (
                    item.orderStatus
                  )}
                </td>
                <td>{item.createdOn}</td>
                <td>
                  <button
                    className='btn btn-primary btn-sm'
                    onClick={() =>
                      editMode[item.id]
                        ? editOrderItem(item.id, orderStatuses[item.id])
                        : toggleEditMode(item.id)
                    }
                  >
                    {editMode[item.id] ? 'Save' : 'Edit'}
                  </button>
                </td>
                <td>
                  <button
                    className='btn btn-danger btn-sm'
                    onClick={() => deleteOrderItem(item.id)}
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

export default AdminOrder
