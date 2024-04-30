import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import AdminNavBar from '../pages/AdminNavBar'

function AdminHome() {
  const [products, setProducts] = useState([])
  const [searchTerm, setSearchTerm] = useState('')
  const [selectedProduct, setSelectedProduct] = useState(null)
  const [showAddProductForm, setShowAddProductForm] = useState(false) // New state variable
  const [newProduct, setNewProduct] = useState({
    // New state variable
    name: '',
    manufacturer: '',
    mrp: '',
    discount: '',
    imageUrl: '',
    status: 0,
  })

  useEffect(() => {
    loadProducts()
  }, [])

  const loadProducts = async () => {
    try {
      const result = await axios.get('https://localhost:7253/api/Product')
      if (result.status === 200) {
        setProducts(result.data)
      } else {
        toast.error(result.error)
      }
    } catch (error) {
      console.error('Error loading products:', error)
      toast.error('Error loading products')
    }
  }

  const getStatusColor = (status) => {
    return status === 0 ? 'text-success' : 'text-danger'
  }

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('en-IN', {
      maximumFractionDigits: 2,
      minimumFractionDigits: 2,
    }).format(value)
  }

  const filteredProducts = products.filter(
    (product) =>
      product.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      product.manufacturer.toLowerCase().includes(searchTerm.toLowerCase())
  )

  const handleSearchTermChange = (event) => {
    setSearchTerm(event.target.value)
  }

  const handleEditProduct = (product) => {
    setSelectedProduct(product)
    const editSection = document.getElementById('editSection')
    if (editSection) {
      editSection.scrollIntoView({ behavior: 'smooth' })
    }
  }

  const handleInputChange = (event) => {
    const { name, value } = event.target;
  
    // If MRP or Discount is changed, recalculate UnitPrice
    if (name === 'mrp' || name === 'discount') {
      const mrp = name === 'mrp' ? parseFloat(value) : parseFloat(selectedProduct.mrp);
      const discount = name === 'discount' ? parseFloat(value) : parseFloat(selectedProduct.discount);
      const unitPrice = mrp - (mrp * discount) / 100;
  
      setSelectedProduct((prevState) => ({
        ...prevState,
        [name]: value,
        unitPrice: unitPrice.toFixed(2), // Round to 2 decimal places
      }));
    } else {
      setSelectedProduct((prevState) => ({
        ...prevState,
        [name]: value,
      }));
    }
  };
  

  const handleUpdateProduct = async () => {
    try {
      const result = await axios.put(
        `https://localhost:7253/api/Product/${selectedProduct.id}`,
        selectedProduct
      )

      if (result.status === 200) {
        toast.success('Product updated successfully')
        loadProducts()
        setSelectedProduct(null) // Reset selected product after update
      } else {
        toast.error('Error updating product')
      }
    } catch (error) {
      console.error('Error updating product:', error)
      toast.error('Error updating product')
    }
  }

  const handleRemoveProduct = async (productId) => {
    try {
      const result = await axios.delete(
        `https://localhost:7253/api/Product/${productId}`
      )

      if (result.status === 200) {
        toast.success('Product removed successfully')
        loadProducts()
      } else {
        toast.error('Error removing product')
      }
    } catch (error) {
      console.error('Error removing product:', error)
      toast.error('Error removing product')
    }
  }

  const handleAddProduct = async () => {
    // Calculate the UnitPrice based on MRP and Discount
    const mrp = parseFloat(newProduct.mrp)
    const discount = parseFloat(newProduct.discount)
    const unitPrice = mrp - (mrp * discount) / 100

    // Update the newProduct object with the calculated UnitPrice
    const updatedProduct = {
      ...newProduct,
      unitPrice: unitPrice.toFixed(2), // Round to 2 decimal places
    }

    // Function to handle form submission
    try {
      const result = await axios.post(
        'https://localhost:7253/api/Product',
        updatedProduct // Use the updated product object with calculated UnitPrice
      )

      if (result.status === 201) {
        toast.success('Product added successfully')
        loadProducts()
        setShowAddProductForm(false)
        setNewProduct({
          // Reset form fields after submission
          name: '',
          manufacturer: '',
          mrp: '',
          discount: '',
          imageUrl: '',
          status: 0,
        })
      } else {
        toast.error('Error adding product')
      }
    } catch (error) {
      console.error('Error adding product:', error)
      toast.error('Error adding product')
    }
  }

  return (
    <div>
      <AdminNavBar />
      <div className='row mx-md-1 mt-4'>
        <div className='col-md-6 mb-3'>
          <button
            className='btn btn-primary'
            onClick={() => setShowAddProductForm(true)}
          >
            Add Product
          </button>
        </div>
        <div className='col-md-6'>
          <div className='searchBox'>
            <div className='input-group'>
              <input
                type='search'
                placeholder='Search by name or manufacturer'
                value={searchTerm}
                onChange={handleSearchTermChange}
                className='form-control border-end-0 rounded-start searchInputBox'
              />
              <button
                className='btn btn-outline-secondary border-start-0 rounded-end p-2'
                type='button'
              >
                🔍
              </button>
            </div>
          </div>
        </div>
      </div>

      <div>
        {showAddProductForm && ( // Conditionally render the form based on state
          <div className='container w-50 mt-4'>
            <div className='card shadow mb-5'>
              <div className='card-body'>
                <h4 className='card-title text-center'>Add Product</h4>
                <div className='form-group'>
                  <label>Product Title : </label>
                  <input
                    type='text'
                    className='form-control'
                    name='name'
                    value={newProduct.name}
                    onChange={(e) =>
                      setNewProduct({ ...newProduct, name: e.target.value })
                    }
                  />
                </div>{' '}
                <br />
                <div className='form-group'>
                  <label>Manufacturer : </label>
                  <input
                    type='text'
                    className='form-control'
                    name='manufacturer'
                    value={newProduct.manufacturer}
                    onChange={(e) =>
                      setNewProduct({
                        ...newProduct,
                        manufacturer: e.target.value,
                      })
                    }
                  />
                </div>{' '}
                <br />
                <div className='form-group'>
                  <label>MRP (₹): </label>
                  <input
                    type='number'
                    className='form-control'
                    name='mrp'
                    value={newProduct.mrp}
                    onChange={(e) =>
                      setNewProduct({ ...newProduct, mrp: e.target.value })
                    }
                  />
                </div>
                <br />
                <div className='form-group'>
                  <label>Discount (%): </label>
                  <input
                    type='number'
                    className='form-control'
                    name='discount'
                    value={newProduct.discount}
                    onChange={(e) =>
                      setNewProduct({ ...newProduct, discount: e.target.value })
                    }
                  />
                </div>
                <br />
                <div className='form-group'>
                  <label>Image URL: </label>
                  <input
                    type='text'
                    className='form-control'
                    name='imageUrl'
                    value={newProduct.imageUrl}
                    onChange={(e) =>
                      setNewProduct({ ...newProduct, imageUrl: e.target.value })
                    }
                  />
                </div>
                <br />
                <div className='form-group'>
                  <label>Availability : </label>
                  <select
                    className='form-control'
                    name='status'
                    value={newProduct.status}
                    onChange={(e) =>
                      setNewProduct({ ...newProduct, status: e.target.value })
                    }
                  >
                    <option value={0}>Available</option>
                    <option value={1}>Unavailable</option>
                  </select>
                </div>
                <br />
                <div className='btn-group'>
                  <button
                    className='btn btn-primary'
                    onClick={handleAddProduct}
                  >
                    Save
                  </button>
                  <button
                    className='btn btn-secondary'
                    onClick={() => setShowAddProductForm(false)}
                  >
                    Cancel
                  </button>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
      {selectedProduct && (
        <div id='editSection' className='container w-50 mt-4'>
          <div className='card shadow mb-5 '>
            <div className='card-body'>
              <h4 className='card-title'>Edit Product</h4>
              <div className='form-group'>
                <label>Name : </label>
                <input
                  type='text'
                  className='form-control'
                  name='name'
                  value={selectedProduct.name}
                  onChange={handleInputChange}
                />
              </div>{' '}
              <br />
              <div className='form-group'>
                <label>Manufacturer : </label>
                <input
                  type='text'
                  className='form-control'
                  name='manufacturer'
                  value={selectedProduct.manufacturer}
                  onChange={handleInputChange}
                />
              </div>
              <br />
              <div className='form-group'>
                <label>MRP : </label>
                <input
                  type='text'
                  className='form-control'
                  name='mrp'
                  value={selectedProduct.mrp}
                  onChange={handleInputChange}
                />
              </div>
              <br />
              <div className='form-group'>
                <label>Discount : </label>
                <input
                  type='text'
                  className='form-control'
                  name='discount'
                  value={selectedProduct.discount}
                  onChange={handleInputChange}
                />
              </div>
              <br />
              <div className='form-group'>
                <label>Image URL : </label>
                <input
                  type='text'
                  className='form-control'
                  name='imageUrl'
                  value={selectedProduct.imageUrl}
                  onChange={handleInputChange}
                />
              </div>
              <br />
              <div className='form-group'>
                <label>Availability : </label>
                <select
                  className='form-control'
                  name='status'
                  value={selectedProduct.status}
                  onChange={handleInputChange}
                >
                  <option value={0}>Available</option>
                  <option value={1}>Unavailable</option>
                </select>
              </div>
              <br />
              <div className='btn-group'>
                <button
                  className='btn btn-primary'
                  onClick={handleUpdateProduct}
                >
                  Update
                </button>
                <button
                  className='btn btn-secondary'
                  onClick={() => setSelectedProduct(null)}
                >
                  Cancel
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
      <div className='row row-cols-1 row-cols-md-2 mx-md-1'>
        {filteredProducts.map((product) => (
          <div key={product.id} className='col mb-4'>
            <div className='card'>
              <div className='row no-gutters'>
                <div className='col-md-5'>
                  <img
                    src={product.imageUrl}
                    className='card-img'
                    alt={product.name}
                    style={{
                      height: '100%',
                      width: '100%',
                      objectFit: 'contain',
                    }}
                  />
                </div>
                <div className='col-md-7'>
                  <div className='card-body'>
                    <h5 className='card-title' title={product.name}>
                      <strong>
                        {product.name.length > 22
                          ? `${product.name.substring(0, 25)}...`
                          : product.name}
                      </strong>
                    </h5>

                    <p className='card-text'>
                      <span style={{ fontSize: '20px' }}>₹ </span>
                      <strong style={{ fontSize: '2rem' }}>
                        {formatCurrency(product.unitPrice)}
                      </strong>{' '}
                      <br />
                      M.R.P : ₹{' '}
                      <span style={{ textDecoration: 'line-through' }}>
                        {formatCurrency(product.mrp)}
                      </span>
                      <br />
                      <b>Manufacturer</b> : {product.manufacturer}
                      <br />
                      <b>Discount</b> : -{product.discount}%<br />
                      <b>Status</b> :{' '}
                      <span className={getStatusColor(product.status)}>
                        <strong>
                          {product.status === 0 ? 'Available' : 'Out of stock'}
                        </strong>
                      </span>
                    </p>
                    <button
                      className='btn btn-primary'
                      style={{ marginRight: '1vw' }}
                      onClick={() => handleEditProduct(product)}
                    >
                      Edit
                    </button>
                    <button
                      className='btn btn-warning'
                      onClick={() => handleRemoveProduct(product.id)}
                    >
                      Remove
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default AdminHome
